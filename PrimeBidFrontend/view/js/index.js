let allProducts = []; // Store all products globally for easy access

// Function to fetch and display all products
async function fetchAllProducts() {
    try {
        const response = await fetch("https://localhost:7217/api/Details/products");

        if (!response.ok) {
            throw new Error(`Failed to fetch products: ${response.statusText}`);
        }

        // Store products in the global array
        allProducts = await response.json();
        displayFilteredProducts(); // Display products initially based on selected categories
    } catch (error) {
        console.error("Error fetching products:", error);
    }
}

// Display products based on selected categories
function displayFilteredProducts() {
    const itemsGrid = document.querySelector(".items-grid");
    itemsGrid.innerHTML = ""; // Clear existing content

    // Get selected categories
    const selectedCategories = Array.from(document.querySelectorAll(".category-list input:checked"))
                                    .map(checkbox => checkbox.value);

    // Filter products by category only
    const filteredProducts = allProducts.filter(product => 
        selectedCategories.includes(product.category.toLowerCase())
    );

    // Generate HTML for each filtered product
    filteredProducts.forEach(product => {
        const auctionCard = document.createElement("div");
        auctionCard.classList.add("auction-card");

        auctionCard.innerHTML = `
            <a href="details.html?id=${product.id}" class="auction-link">
            <div class = "iContainer">
                <div class="image-container">
                    <img src="${product.itemImage || 'Resources/placeholder.jpg'}" alt="${product.itemName}">
                </div>
                <div class="title-wishlist">
                    <p class="card-title">${product.itemName}</p>
                    </a>
                    <div class="wishlist-icon">
                        <img src="Resources/wishlistIcon.svg" alt="Wishlist">
                    </div>
                </div>
                <div class="price-row">
                    <p class="price">$${product.estimatedBid} USD</p>
                </div>
                <div class="bid-time-row">
                    <p class="current-bid">Current Bid:</p>
                    <p class="time-label">Remaining Time:</p>
                </div>
                <div class="bid-time-values">
                    <p class="next-bid">$${product.bidAmount || 'N/A'} USD</p>
                    <p class="remaining-time">${new Date(product.endDate).toLocaleString()}</p>
                </div>
            </div>
        `;

        itemsGrid.appendChild(auctionCard);
    });
}

// Attach event listeners to category checkboxes
document.querySelectorAll(".category-list input").forEach(checkbox => {
    checkbox.addEventListener("change", displayFilteredProducts);
});

// Fetch and display products on page load
window.addEventListener("DOMContentLoaded", fetchAllProducts);
