let allProducts = []; // Store all products globally for easy access

// Function to fetch and display all products
async function fetchAllProducts() {
    try {
        const response = await fetch("https://localhost:7217/api/Details/products");

        if (!response.ok) {
            throw new Error(`Failed to fetch products: ${response.statusText}`);
        }

        allProducts = await response.json();
        setInitialCategorySelection();
        displayFilteredProducts();
    } catch (error) {
        console.error("Error fetching products:", error);
    }
}

// Get category from URL and check the corresponding category checkbox
function setInitialCategorySelection() {
    const urlParams = new URLSearchParams(window.location.search);
    const selectedCategory = urlParams.get("category");

    if (selectedCategory) {
        document.querySelectorAll(".category-list input").forEach(checkbox => {
            checkbox.checked = checkbox.value.toLowerCase() === selectedCategory.toLowerCase();
        });
    }
}

// Display products based on selected categories
function displayFilteredProducts() {
    const itemsGrid = document.querySelector(".items-grid");
    itemsGrid.innerHTML = ""; // Clear existing content

    const selectedCategories = Array.from(document.querySelectorAll(".category-list input:checked"))
                                    .map(checkbox => checkbox.value.toLowerCase());

    const filteredProducts = allProducts.filter(product =>
        selectedCategories.includes(product.category.toLowerCase())
    );

    filteredProducts.forEach(product => {
        const auctionCard = document.createElement("div");
        auctionCard.classList.add("auction-card");

        auctionCard.innerHTML = `
            <a href="details.html?id=${product.id}" class="auction-link">
            <div class="iContainer">
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

// Handle category checkbox changes, uncheck all others when one is selected
document.querySelectorAll(".category-list input").forEach(checkbox => {
    checkbox.addEventListener("change", function() {
        if (this.checked) {
            document.querySelectorAll(".category-list input").forEach(otherCheckbox => {
                if (otherCheckbox !== this) {
                    otherCheckbox.checked = false;
                }
            });
        }
        displayFilteredProducts();
    });
});

// Fetch and display products on page load
window.addEventListener("DOMContentLoaded", () => {
    fetchAllProducts();
});

// Additional functions for fetching soon-to-end and popular auctions
async function fetchAuctions(endpoint) {
    try {
        const response = await fetch(endpoint);
        if (!response.ok) throw new Error(`Error fetching data: ${response.statusText}`);
        return await response.json();
    } catch (error) {
        console.error("Failed to fetch auctions:", error);
        return [];
    }
}

function displayAuctions(auctions, containerId) {
    const container = document.getElementById(containerId);
    container.innerHTML = '';

    auctions.forEach(auction => {
        const auctionCard = document.createElement("div");
        auctionCard.classList.add("auction-card");

        auctionCard.innerHTML = `
            <a href="details.html?id=${auction.id}" class="auction-link">
                <div class="image-container">
                    <img src="${auction.itemImage || 'Resources/placeholder.jpg'}" alt="${auction.itemName}">
                </div>
            </a>
            <div class="title-wishlist">
                <p class="card-title">${auction.itemName}</p>
                <div class="wishlist-icon">
                    <img src="Resources/wishlistIcon.svg" alt="Wishlist">
                </div>
            </div>
            <div class="price-row">
                <p class="price">$${auction.estimatedBid} USD</p>
            </div>
            <div class="bid-time-row">
                <p class="current-bid">Current Bid:</p>
                <p class="time-label">Remaining Time:</p>
            </div>
            <div class="bid-time-values">
                <p class="next-bid">$${auction.bidAmount || 'N/A'} USD</p>
                <p class="remaining-time">${new Date(auction.endDate).toLocaleString()}</p>
            </div>
        `;
        container.appendChild(auctionCard);
    });
}


// Fetch soon-to-end and popular auctions on page load
window.addEventListener("DOMContentLoaded", async () => {
    const soonToEndAuctions = await fetchAuctions("https://localhost:7217/api/Details/soon-to-end");
    const popularAuctions = await fetchAuctions("https://localhost:7217/api/Details/popular");

    displayAuctions(soonToEndAuctions, "soonToEndAuctions");
    displayAuctions(popularAuctions, "popularAuctions");
});

// Select All functionality
const selectAllCheckbox = document.getElementById("select-all");
selectAllCheckbox.addEventListener("change", function () {
    const categoryCheckboxes = document.querySelectorAll(".category-list input[type='checkbox']");
    
    categoryCheckboxes.forEach(checkbox => {
        checkbox.checked = this.checked; // Set each checkbox to match the "Select All" checkbox state
    });

    displayFilteredProducts(); // Update the displayed products based on the newly selected categories
});

// Handle individual category checkbox changes
document.querySelectorAll(".category-list input").forEach(checkbox => {
    checkbox.addEventListener("change", function () {
        if (!this.checked) {
            selectAllCheckbox.checked = false; // Uncheck "Select All" if any checkbox is unchecked
        }

        // Check if all individual checkboxes are checked to update "Select All" checkbox
        const allChecked = Array.from(document.querySelectorAll(".category-list input")).every(cb => cb.checked);
        selectAllCheckbox.checked = allChecked; // Set "Select All" to checked if all are checked
        displayFilteredProducts(); // Update product display based on the current selection
    });
});