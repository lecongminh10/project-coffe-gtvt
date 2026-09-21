// ==========================================================================
// Coffee Paradise - Global Client Helper Functions
// ==========================================================================

// Global Coffee Toast Notification
function showCoffeeToast(title, message, type = 'success') {
    let container = document.getElementById('globalCoffeeToastContainer');
    if (!container) {
        container = document.createElement('div');
        container.id = 'globalCoffeeToastContainer';
        container.className = 'coffee-toast-container';
        document.body.appendChild(container);
    }

    const toastId = 'toast_' + Date.now();
    let iconClass = 'fa-check';
    let iconBg = '#fef3c7';
    let iconColor = '#d97706';

    if (type === 'error' || type === 'danger') {
        iconClass = 'fa-exclamation';
        iconBg = '#fee2e2';
        iconColor = '#ef4444';
    } else if (type === 'info') {
        iconClass = 'fa-info';
        iconBg = '#e0f2fe';
        iconColor = '#0284c7';
    }

    const toast = document.createElement('div');
    toast.id = toastId;
    toast.className = 'coffee-toast-item';
    toast.innerHTML = `
        <div class="coffee-toast-icon" style="background: ${iconBg}; color: ${iconColor};">
            <i class="fas ${iconClass}"></i>
        </div>
        <div class="coffee-toast-content">
            <div class="coffee-toast-title">${title}</div>
            <p class="coffee-toast-msg">${message}</p>
        </div>
        <button class="coffee-toast-close" onclick="dismissToast('${toastId}')">
            <i class="fas fa-times"></i>
        </button>
    `;

    container.appendChild(toast);

    // Auto dismiss after 3.5s
    setTimeout(() => {
        dismissToast(toastId);
    }, 3500);
}

function dismissToast(toastId) {
    const el = document.getElementById(toastId);
    if (el) {
        el.style.opacity = '0';
        el.style.transform = 'translateX(100%)';
        setTimeout(() => el.remove(), 300);
    }
}

// Global Add to Cart function with animated counter badge and toast
function addToCartGlobal(productId, quantity = 1, productName = '') {
    $.ajax({
        url: '/Cart/AddToCartAjax',
        type: 'POST',
        data: { productId: productId, quantity: quantity },
        success: function (res) {
            if (res.success) {
                // Update badge with pulse effect
                const badge = $('#cartBadgeCount');
                badge.text(res.totalCount);
                badge.addClass('animate__animated animate__pulse');
                setTimeout(() => badge.removeClass('animate__animated animate__pulse'), 600);

                showCoffeeToast(
                    'Đã thêm vào giỏ!', 
                    productName ? `"${productName}" đã sẵn sàng trong giỏ hàng.` : res.message, 
                    'success'
                );
            } else {
                showCoffeeToast('Thông báo', res.message || 'Không thể thêm món.', 'danger');
            }
        },
        error: function () {
            showCoffeeToast('Lỗi kết nối', 'Không thể kết nối đến máy chủ. Vui lòng thử lại.', 'danger');
        }
    });
}
