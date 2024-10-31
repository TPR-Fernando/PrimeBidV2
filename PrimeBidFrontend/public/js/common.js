document.addEventListener("DOMContentLoaded", function () {
    // Load the navbar HTML
    fetch('navbar.html')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            }
            return response.text();
        })
        .then(data => {
            // Create a container for the navbar and insert it at the top of the body
            const bodyElement = document.querySelector('body');
            const navbarWrapper = document.createElement('div');
            navbarWrapper.innerHTML = data;
            bodyElement.insertBefore(navbarWrapper, bodyElement.firstChild);

            // Add the navbar.css after the navbar HTML is loaded
            const navbarLink = document.createElement('link');
            navbarLink.rel = 'stylesheet';
            navbarLink.href = 'css/navbar.css';
            document.head.appendChild(navbarLink);

            // Set the active link based on the current URL
            const navLinks = document.querySelectorAll('.nav-links a');
            const currentPath = window.location.pathname.split('/').pop(); // Get just the filename
            const activeLink = currentPath || 'index.html'; // If currentPath is empty, default to index.html

            navLinks.forEach(link => {
                const linkHref = link.getAttribute('href'); // Get the href attribute of the link
                if (linkHref === activeLink) {
                    link.classList.add('active'); // Add active class to the matching link
                }
            });

            // Check if the session is active
            let isLoggedIn = false; // Check session storage (or your preferred method)

            let email = getCookie("email");
            if (email) {
                console.log("Welcome back, " + email + "!");
                isLoggedIn = true;
            } else {
                console.log("No user logged in.");
            }

            if (isLoggedIn) {
                let signInButton = document.querySelector('.sign-in-button'); // Select the Sign In button
                if (signInButton) {
                    signInButton.style.display = 'none'; // Hide the Sign In button
                }
            }
        })
        .catch(error => console.error('Error loading navbar:', error));

    // Load the footer HTML
    fetch('Footer.html')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            }
            return response.text();
        })
        .then(data => {
            // Create a container for the footer and insert it at the bottom of the body
            const bodyElement = document.querySelector('body');
            const footerWrapper = document.createElement('div');
            footerWrapper.innerHTML = data;
            bodyElement.appendChild(footerWrapper);

            // Add the Footer.css after the footer HTML is loaded
            const footerLink = document.createElement('link');
            footerLink.rel = 'stylesheet';
            footerLink.href = 'css/Footer.css';
            document.head.appendChild(footerLink);
        })
        .catch(error => console.error('Error loading footer:', error));
});

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}
