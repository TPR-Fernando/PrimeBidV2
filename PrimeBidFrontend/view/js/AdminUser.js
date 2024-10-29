// Base API URL
const apiUrl = "https://localhost:7217/api/Profile/profiles"; // Ensure this is correct

// Utility function to handle fetch requests with error handling
async function apiFetch(url, options = {}) {
    try {
        const response = await fetch(url, options);
        if (!response.ok) {
            const errorMessage = await response.text();
            throw new Error(`Request failed: ${response.status} ${response.statusText} - ${errorMessage}`);
        }
        return await response.json();
    } catch (error) {
        console.error("API error:", error);
        alert("Operation failed. Please try again. Error details: " + error.message);
    }
}

// Function to fetch all profiles
async function fetchAllProfiles() {
    const profiles = await apiFetch(apiUrl);
    if (profiles) {
        const tableBody = document.querySelector(".user-table tbody");
        tableBody.innerHTML = ""; // Clear previous rows
        profiles.forEach(renderProfileRow);
    }
}

// Function to render profile rows in the table
function renderProfileRow(profile) {
    const tableBody = document.querySelector(".user-table tbody");
    const row = document.createElement("tr");
    row.setAttribute("data-profile-id", profile.id);
    row.innerHTML = `
        <td>${profile.id}</td>
        <td>${profile.fullName}</td>
        <td>${profile.email}</td>
        <td>${profile.address}</td>
        <td>${profile.phoneNumber}</td>
        
        <td>
            <button class="btn delete-btn" onclick="deleteProfile(${profile.id})">Delete</button>
        </td>
    `;
    tableBody.appendChild(row);
}

// Function to delete a profile
// Function to delete a profile
async function deleteProfile(profileId) {
    if (confirm("Are you sure you want to delete this profile?")) {
        const deleteUrl = `${apiUrl}/${profileId}`; // Construct the DELETE URL
        console.log(`Deleting profile at: ${deleteUrl}`); // Log the URL for debugging
        
        const response = await apiFetch(deleteUrl, { method: "DELETE" });
        
        if (response) {
            // Remove the profile row from the table
            document.querySelector(`tr[data-profile-id="${profileId}"]`).remove();
            alert("Profile deleted successfully!");
        }
    }
}




// Initialize fetching on page load
document.addEventListener("DOMContentLoaded", fetchAllProfiles);
