// Function to show popup when field is set to "yes"
function checkFieldAndShowPopup(fieldValue) {
    if (fieldValue && fieldValue.toLowerCase() === 'yes') {
        showPopup('Field is set to Yes!');
    }
}

// Function to display popup
function showPopup(message) {
    // Using alert - simple popup
    alert(message);
    
    // Alternative: Using a modal popup (uncomment to use)
    // displayModalPopup(message);
}

// Alternative: Display a custom modal popup
function displayModalPopup(message) {
    const popup = document.createElement('div');
    popup.id = 'popup-modal';
    popup.style.cssText = `
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        background-color: white;
        border: 2px solid #333;
        border-radius: 8px;
        padding: 20px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        z-index: 1000;
        max-width: 400px;
        text-align: center;
    `;
    
    popup.innerHTML = `
        <p style="font-size: 16px; margin: 0 0 20px 0;">${message}</p>
        <button onclick="closePopup()" style="
            padding: 10px 20px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        ">Close</button>
    `;
    
    document.body.appendChild(popup);
    
    // Add overlay
    const overlay = document.createElement('div');
    overlay.id = 'popup-overlay';
    overlay.style.cssText = `
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5);
        z-index: 999;
    `;
    document.body.appendChild(overlay);
}

// Function to close the modal popup
function closePopup() {
    const popup = document.getElementById('popup-modal');
    const overlay = document.getElementById('popup-overlay');
    
    if (popup) popup.remove();
    if (overlay) overlay.remove();
}

// Example: Listen for input field changes
document.addEventListener('DOMContentLoaded', function() {
    const inputField = document.getElementById('myField');
    
    if (inputField) {
        inputField.addEventListener('change', function() {
            checkFieldAndShowPopup(this.value);
        });
    }
});
