// FXTransfer JavaScript utilities

// Toast notification system
window.showToast = function (message, type = 'info') {
    const toastContainer = document.getElementById('toast-container');
    if (!toastContainer) {
        const container = document.createElement('div');
        container.id = 'toast-container';
        container.className = 'toast-container';
        document.body.appendChild(container);
    }

    const toastId = 'toast-' + Date.now();
    const toastHtml = `
        <div id="${toastId}" class="toast" role="alert" aria-live="assertive" aria-atomic="true" data-bs-autohide="true" data-bs-delay="5000">
            <div class="toast-header">
                <strong class="me-auto">FXTransfer</strong>
                <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        </div>
    `;

    const toastContainerDiv = document.getElementById('toast-container');
    toastContainerDiv.insertAdjacentHTML('beforeend', toastHtml);

    const toastElement = document.getElementById(toastId);
    const toast = new bootstrap.Toast(toastElement);
    toast.show();

    toastElement.addEventListener('hidden.bs.toast', function () {
        toastElement.remove();
    });
};

// Loading overlay
window.showLoading = function (show) {
    let overlay = document.getElementById('loading-overlay');
    if (show) {
        if (!overlay) {
            overlay = document.createElement('div');
            overlay.id = 'loading-overlay';
            overlay.className = 'loading-overlay';
            overlay.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>';
            document.body.appendChild(overlay);
        }
        overlay.style.display = 'flex';
    } else if (overlay) {
        overlay.style.display = 'none';
    }
};

// Format currency
window.formatCurrency = function (amount, currency = 'USD') {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: currency,
        minimumFractionDigits: 2
    }).format(amount);
};

// Copy to clipboard
window.copyToClipboard = async function (text) {
    try {
        await navigator.clipboard.writeText(text);
        window.showToast('Copied to clipboard!', 'success');
        return true;
    } catch (err) {
        console.error('Failed to copy: ', err);
        window.showToast('Failed to copy', 'error');
        return false;
    }
};

// Geolocation
window.getCurrentPosition = function () {
    return new Promise((resolve, reject) => {
        if (!navigator.geolocation) {
            reject('Geolocation not supported');
            return;
        }

        navigator.geolocation.getCurrentPosition(
            position => {
                resolve({
                    latitude: position.coords.latitude,
                    longitude: position.coords.longitude
                });
            },
            error => {
                reject(error.message);
            }
        );
    });
};

// Scroll to top
window.scrollToTop = function () {
    window.scrollTo({ top: 0, behavior: 'smooth' });
};

// Confirm dialog
window.confirmAction = function (message) {
    return confirm(message);
};