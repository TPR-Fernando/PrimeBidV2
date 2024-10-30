// Admin.js

// Base API URL
const apiUrl = "https://localhost:7217/api/Details/products";

// Utility function to handle fetch requests with error handling
async function apiFetch(url, options = {}) {
    try {
        const response = await fetch(url, options);
        if (!response.ok) {
            throw new Error(`Request failed: ${response.statusText}`);
        }
        return await response.json();
    } catch (error) {
        console.error("API error:", error);
        alert("Operation failed. Please try again.");
    }
}

// Fetch all products and render them in the table
async function fetchAllProducts() {
    const products = await apiFetch(apiUrl);
    if (products) {
        const tableBody = document.querySelector(".user-table tbody");
        tableBody.innerHTML = ""; // Clear previous rows
        products.forEach(renderProductRow);
    }
}

// Render a single product row in the table
function renderProductRow(product) {
    const tableBody = document.querySelector(".user-table tbody");
    const row = document.createElement("tr");
    row.setAttribute("data-product-id", product.id);
    row.innerHTML = `
        <td>${product.id}</td>
        <td>${product.itemName}</td>
        <td><img src="${product.itemImage || 'Resources/placeholder.jpg'}" alt="${product.itemName}" width="50" height="50"></td>
        <td>${product.itemDescription}</td>
        <td>${product.category}</td>
        <td>$${product.estimatedBid}</td>
        <td>$${product.bidAmount || '0'}</td>
        <td>${new Date(product.endDate).toLocaleString()}</td>
        <td>
            <button class="btn edit-btn" onclick="editProduct(${product.id})">Edit</button>
            <button class="btn delete-btn" onclick="deleteProduct(${product.id})">Delete</button>
        </td>
    `;
    tableBody.appendChild(row);
}

// Delete a product and remove its row
async function deleteProduct(productId) {
    if (confirm("Are you sure you want to delete this item?")) {
        const deleted = await apiFetch(`${apiUrl}/${productId}`, { method: "DELETE" });
        if (deleted) {
            document.querySelector(`tr[data-product-id="${productId}"]`).remove();
            alert("Product deleted successfully!");
        }
    }
}

// Redirect to the edit page with the selected product ID
function editProduct(productId) {
    window.location.href = `AdminAuctionEdit.html?id=${productId}`;
}


// On DOMContentLoaded, initialize fetch for product list
document.addEventListener("DOMContentLoaded", fetchAllProducts);
