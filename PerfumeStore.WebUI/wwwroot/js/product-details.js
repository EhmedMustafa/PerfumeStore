// Şəkil qalereyası funksionallığı
document.addEventListener('DOMContentLoaded', function() {
    // Əsas şəkil və kiçik şəkillər
    const mainImage = document.querySelector('.main-image img');
    const thumbnails = document.querySelectorAll('.thumbnail-list img');

    // Kiçik şəkillərə klik hadisəsi əlavə et
    thumbnails.forEach(thumb => {
        thumb.addEventListener('click', function() {
            // Əsas şəkli dəyiş
            mainImage.src = this.src;
            
            // Aktiv şəkli vurğula
            thumbnails.forEach(t => t.classList.remove('active'));
            this.classList.add('active');
        });
    });

    // Tab funksionallığı
    const tabButtons = document.querySelectorAll('.tab-btn');
    const tabPanes = document.querySelectorAll('.tab-pane');

    tabButtons.forEach(button => {
        button.addEventListener('click', function() {
            // Aktiv tab düyməsini dəyiş
            tabButtons.forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');

            // Aktiv tab məzmununu göstər
            const targetPane = this.getAttribute('data-tab');
            tabPanes.forEach(pane => {
                pane.classList.remove('active');
                if (pane.id === targetPane) {
                    pane.classList.add('active');
                }
            });
        });
    });

    // Məhsul ölçüləri və miqdar seçimi
    const sizeOptions = document.querySelectorAll('.size-option');
    const totalAmount = document.querySelector('.total-amount');
    let total = 0;

    // Miqdar düymələrinin funksionallığı
    sizeOptions.forEach(option => {
        const minusBtn = option.querySelector('.minus');
        const plusBtn = option.querySelector('.plus');
        const input = option.querySelector('.quantity-input');
        const checkbox = option.querySelector('input[type="checkbox"]');
        const price = parseInt(option.querySelector('.size-price').textContent);

        // Miqdarı azaltma
        minusBtn.addEventListener('click', () => {
            if (input.value > 0) {
                input.value = parseInt(input.value) - 1;
                updateTotal();
            }
        });

        // Miqdarı artırma
        plusBtn.addEventListener('click', () => {
            if (input.value < 10) {
                input.value = parseInt(input.value) + 1;
                updateTotal();
            }
        });

        // Miqdarı birbaşa daxil etmə
        input.addEventListener('change', () => {
            if (input.value < 0) input.value = 0;
            if (input.value > 10) input.value = 10;
            updateTotal();
        });

        // Checkbox dəyişikliyi
        checkbox.addEventListener('change', () => {
            if (!checkbox.checked) {
                input.value = 0;
            }
            updateTotal();
        });
    });

    // Ümumi məbləği yeniləmə
    function updateTotal() {
        total = 0;
        sizeOptions.forEach(option => {
            const checkbox = option.querySelector('input[type="checkbox"]');
            const input = option.querySelector('.quantity-input');
            const price = parseInt(option.querySelector('.size-price').textContent);
            
            if (checkbox.checked) {
                total += price * parseInt(input.value);
            }
        });
        
        totalAmount.textContent = `${total} ₼`;
    }

    // Səbətə əlavə et düyməsi
    const addToCartBtn = document.querySelector('.btn-add-to-cart');
    if (addToCartBtn) {
        addToCartBtn.addEventListener('click', function() {
            // Səbətə əlavə et funksionallığı
            const productName = document.querySelector('.product-header h1').textContent;
            alert(`${productName} səbətə əlavə edildi!`);
        });
    }

    // İstək siyahısı düyməsi
    const wishlistBtn = document.querySelector('.btn-wishlist');
    if (wishlistBtn) {
        wishlistBtn.addEventListener('click', function() {
            // İstək siyahısına əlavə et funksionallığı
            const productName = document.querySelector('.product-header h1').textContent;
            alert(`${productName} istək siyahısına əlavə edildi!`);
        });
    }

    // Rəy yazma funksionallığı
    const reviewForm = document.querySelector('.review-form');
    if (reviewForm) {
        reviewForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            // Rəy məlumatlarını al
            const name = this.querySelector('input[name="name"]').value;
            const rating = this.querySelector('input[name="rating"]:checked').value;
            const comment = this.querySelector('textarea[name="comment"]').value;

            // Yeni rəy elementi yarat
            const reviewItem = document.createElement('div');
            reviewItem.className = 'review-item';
            reviewItem.innerHTML = `
                <div class="review-header">
                    <span class="reviewer-name">${name}</span>
                    <span class="review-date">${new Date().toLocaleDateString('az-AZ')}</span>
                </div>
                <div class="review-rating">
                    ${'★'.repeat(rating)}${'☆'.repeat(5-rating)}
                </div>
                <div class="review-text">${comment}</div>
            `;

            // Rəyi əlavə et
            const reviewsList = document.querySelector('.reviews-list');
            reviewsList.insertBefore(reviewItem, reviewsList.firstChild);

            // Formu təmizlə
            this.reset();
        });
    }

    // Paylaşma düymələri funksionallığı
    const shareButtons = document.querySelectorAll('.share-buttons-container .buttons a');
    const currentUrl = window.location.href;
    const productName = document.querySelector('.product-header h1').textContent.trim();

    // WhatsApp paylaşımı
    shareButtons[0].href = `https://wa.me/?text=${encodeURIComponent(productName + ' - ' + currentUrl)}`;

    // Facebook paylaşımı
    shareButtons[1].href = `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(currentUrl)}`;

    // Twitter paylaşımı
 

    // Telegram paylaşımı
    shareButtons[3].href = `https://t.me/share/url?url=${encodeURIComponent(currentUrl)}&text=${encodeURIComponent(productName)}`;
}); 