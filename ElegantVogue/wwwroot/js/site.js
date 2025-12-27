// ========================================
// SITE.JS - Elegant Vogue
// ========================================

document.addEventListener('DOMContentLoaded', function () {

    // ========================================
    // FILTER COLLAPSIBLE
    // ========================================
    const collapsibleGroups = document.querySelectorAll('.filter-group.collapsible');

    collapsibleGroups.forEach(group => {
        const label = group.querySelector('.filter-label');

        label.addEventListener('click', function () {
            group.classList.toggle('open');
        });
    });

    // ========================================
    // MENU TOGGLE
    // ========================================
    const menuBtn = document.getElementById('menuBtn');

    if (menuBtn) {
        menuBtn.addEventListener('click', function () {
            this.classList.toggle('active');
            // Add mobile menu logic here
        });
    }

    // ========================================
    // QUANTITY BUTTONS (AJAX VERSION)
    // ========================================
    const updateCartCount = async () => {
        try {
            const response = await fetch('/Cart/GetCartCount');
            const data = await response.json();

            const countElement = document.querySelector('.cart-count');
            if (countElement) {
                countElement.textContent = data.count;
                countElement.style.display = data.count > 0 ? 'flex' : 'none';
            }
        } catch (error) {
            console.error('Error updating cart count:', error);
        }
    };

    // ========================================
    // TERMS CHECKBOX - CART PAGE
    // ========================================
    const termsCheck = document.getElementById('termsCheck');
    const btnContinue = document.getElementById('btnContinue');

    if (termsCheck && btnContinue) {
        // Initial state
        btnContinue.classList.add('disabled');

        termsCheck.addEventListener('change', function () {
            if (this.checked) {
                btnContinue.classList.remove('disabled');
            } else {
                btnContinue.classList.add('disabled');
            }
        });

        btnContinue.addEventListener('click', function (e) {
            if (!termsCheck.checked) {
                e.preventDefault();
                alert('Please agree to the Terms and Conditions');
            }
        });
    }

    // ========================================
    // IMAGE LAZY LOADING
    // ========================================
    const lazyImages = document.querySelectorAll('img[data-src]');

    if ('IntersectionObserver' in window) {
        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.dataset.src;
                    img.removeAttribute('data-src');
                    observer.unobserve(img);
                }
            });
        });

        lazyImages.forEach(img => imageObserver.observe(img));
    } else {
        // Fallback for browsers without IntersectionObserver
        lazyImages.forEach(img => {
            img.src = img.dataset.src;
            img.removeAttribute('data-src');
        });
    }

    // ========================================
    // SMOOTH SCROLL
    // ========================================
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });

    // ========================================
    // FORM VALIDATION FEEDBACK
    // ========================================
    const formInputs = document.querySelectorAll('.form-input');

    formInputs.forEach(input => {
        input.addEventListener('blur', function () {
            if (this.value.trim() !== '') {
                this.classList.add('filled');
            } else {
                this.classList.remove('filled');
            }
        });

        input.addEventListener('invalid', function () {
            this.classList.add('error');
        });

        input.addEventListener('input', function () {
            this.classList.remove('error');
        });
    });

    // ========================================
    // PRODUCT COLOR SELECTION VISUAL FEEDBACK
    // ========================================
    const colorOptions = document.querySelectorAll('.color-option input[type="radio"]');

    colorOptions.forEach(radio => {
        radio.addEventListener('change', function () {
            // Remove active class from all swatches in this group
            const parent = this.closest('.color-options');
            parent.querySelectorAll('.color-swatch-lg').forEach(swatch => {
                swatch.style.borderColor = 'transparent';
            });

            // Add active style to selected
            if (this.checked) {
                this.nextElementSibling.style.borderColor = '#000';
            }
        });
    });

    // ========================================
    // SIZE SELECTION VISUAL FEEDBACK
    // ========================================
    const sizeOptions = document.querySelectorAll('.size-option input[type="radio"]');

    sizeOptions.forEach(radio => {
        radio.addEventListener('change', function () {
            const parent = this.closest('.size-options');
            parent.querySelectorAll('.size-text').forEach(text => {
                text.style.background = '';
                text.style.color = '';
                text.style.borderColor = '';
            });

            if (this.checked) {
                const sizeText = this.nextElementSibling;
                sizeText.style.background = '#000';
                sizeText.style.color = '#fff';
                sizeText.style.borderColor = '#000';
            }
        });
    });

    // ========================================
    // ADD TO CART ANIMATION
    // ========================================
    const addToCartForms = document.querySelectorAll('form[action*="Cart/Add"]');

    addToCartForms.forEach(form => {
        form.addEventListener('submit', function (e) {
            const btn = this.querySelector('button[type="submit"]');
            if (btn) {
                const originalText = btn.textContent;
                btn.textContent = 'Adding...';
                btn.disabled = true;

                setTimeout(() => {
                    btn.textContent = originalText;
                    btn.disabled = false;
                }, 1000);
            }
        });
    });

    // ========================================
    // FILTER TABS
    // ========================================
    const filterTabs = document.querySelectorAll('.filter-tabs .tab');

    filterTabs.forEach(tab => {
        tab.addEventListener('click', function () {
            filterTabs.forEach(t => t.classList.remove('active'));
            this.classList.add('active');

            // Add filter logic here based on tab content
            const category = this.textContent.replace(/[()]/g, '').trim();
            console.log('Filter by category:', category);
        });
    });

    // ========================================
    // WISHLIST BUTTON
    // ========================================
    const wishlistBtns = document.querySelectorAll('.btn-wishlist, .item-wishlist');

    wishlistBtns.forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            this.classList.toggle('active');

            const svg = this.querySelector('svg');
            if (this.classList.contains('active')) {
                svg.setAttribute('fill', 'currentColor');
            } else {
                svg.setAttribute('fill', 'none');
            }
        });
    });

});

// ========================================
// UTILITY FUNCTIONS
// ========================================

// Format price
function formatPrice(price) {
    return '$' + price.toFixed(2);
}

// Debounce function for search
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Show notification
function showNotification(message, type = 'success') {
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;
    notification.textContent = message;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.classList.add('show');
    }, 10);

    setTimeout(() => {
        notification.classList.remove('show');
        setTimeout(() => {
            notification.remove();
        }, 300);
    }, 3000);
}
```

---

## ÉTAPE 12 : Structure des Images

Crée les dossiers suivants dans `wwwroot / images / ` :
```
wwwroot /
└── images /
    ├── products /
    │   ├── tshirt - black - abstract.jpg
    │   ├── tshirt - cream - basic.jpg
    │   ├── tshirt - black - heavy.jpg
    │   ├── shirt - pattern.jpg
    │   ├── tshirt - wave.jpg
    │   ├── tshirt - ink.jpg
    │   ├── tshirt - henley.jpg
    │   ├── jeans - white.jpg
    │   └── tshirt - camo.jpg
    └── banners /
        ├── approach - 1.jpg
        ├── approach - 2.jpg
        ├── approach - 3.jpg
        └── approach - 4.jpg