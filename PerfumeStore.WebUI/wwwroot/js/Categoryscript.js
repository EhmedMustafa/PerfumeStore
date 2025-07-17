/**
 * E-ticarət səhifəsinin əsas JavaScript kodu
 * Bu kod filter sistemi, məhsul interaksiyaları və səbət funksionallığını idarə edir
 */

// DOM elementlərinin seçilməsi - səhifədəki əsas elementlər
const filterToggle = document.querySelector('.filter-toggle'); // Filter panelini açmaq üçün düymə
const filtersSidebar = document.querySelector('.filters-sidebar'); // Filter paneli
const closeFilter = document.querySelector('.close-filter'); // Filter panelini bağlamaq üçün düymə
const productGrid = document.querySelector('.product-grid'); // Məhsulların grid konteyneri
const priceRange = document.getElementById('price-range'); // Qiymət aralığı slideri
const minPriceInput = document.querySelector('.price-inputs input:first-child'); // Minimum qiymət inputu
const maxPriceInput = document.querySelector('.price-inputs input:last-child'); // Maksimum qiymət inputu
const favoriteButtons = document.querySelectorAll('.favorite-btn'); // Favori düymələri
const addToCartButtons = document.querySelectorAll('.add-to-cart'); // Səbətə əlavə etmə düymələri

// Filter panelini açmaq/bağlamaq üçün event listener-lar
filterToggle.addEventListener('click', () => {
    filtersSidebar.classList.add('active');
    document.body.style.overflow = 'hidden'; // Scroll-lock aktivləşdirir
});

closeFilter.addEventListener('click', () => {
    filtersSidebar.classList.remove('active');
    document.body.style.overflow = ''; // Scroll-lock deaktiv edir
});

// ESC düyməsi ilə filter panelini bağlamaq
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && filtersSidebar.classList.contains('active')) {
        filtersSidebar.classList.remove('active');
        document.body.style.overflow = '';
    }
});

// Qiymət slider funksionallığı üçün elementlər
const minRange = document.getElementById('min-price-range');
const maxRange = document.getElementById('max-price-range');
const minPrice = document.getElementById('min-price');
const maxPrice = document.getElementById('max-price');
const rangeFill = document.querySelector('.range-fill');

/**
 * Qiymət aralığının vizual göstəricisini yeniləyir
 * Slider dəyərlərinə əsasən doldurulmuş hissəni hesablayır
 */
function updateRangeFill() {
    const min = parseInt(minRange.value);
    const max = parseInt(maxRange.value);
    const percent1 = (min / 500) * 100;
    const percent2 = (max / 500) * 100;
    rangeFill.style.left = percent1 + '%';
    rangeFill.style.width = (percent2 - percent1) + '%';
}

/**
 * Qiymət inputlarını slider dəyərləri ilə sinxronlaşdırır
 */
function updatePriceInputs() {
    minPrice.value = minRange.value;
    maxPrice.value = maxRange.value;
    updateRangeFill();
}

// Minimum qiymət slideri üçün event listener
minRange.addEventListener('input', () => {
    const min = parseInt(minRange.value);
    const max = parseInt(maxRange.value);
    
    if (min > max) {
        minRange.value = max;
        minPrice.value = max;
    } else {
        minPrice.value = min;
    }
    updateRangeFill();
    filterProducts();
});

// Maksimum qiymət slideri üçün event listener
maxRange.addEventListener('input', () => {
    const min = parseInt(minRange.value);
    const max = parseInt(maxRange.value);
    
    if (max < min) {
        maxRange.value = min;
        maxPrice.value = min;
    } else {
        maxPrice.value = max;
    }
    updateRangeFill();
    filterProducts();
});

// Minimum qiymət inputu üçün event listener
minPrice.addEventListener('change', () => {
    let value = parseInt(minPrice.value);
    if (value < 0) value = 0;
    if (value > 500) value = 500;
    if (value > parseInt(maxPrice.value)) value = parseInt(maxPrice.value);
    
    minPrice.value = value;
    minRange.value = value;
    updateRangeFill();
    filterProducts();
});

// Maksimum qiymət inputu üçün event listener
maxPrice.addEventListener('change', () => {
    let value = parseInt(maxPrice.value);
    if (value < 0) value = 0;
    if (value > 500) value = 500;
    if (value < parseInt(minPrice.value)) value = parseInt(minPrice.value);
    
    maxPrice.value = value;
    maxRange.value = value;
    updateRangeFill();
    filterProducts();
});

/**
 * Favorilərə əlavə etmə funksionallığı
 * Düyməyə kliklədikdə favori statusunu dəyişir və ikonu yeniləyir
 */
favoriteButtons.forEach(button => {
    button.addEventListener('click', (e) => {
        e.preventDefault();
        button.classList.toggle('active');
        const icon = button.querySelector('i');
        if (icon) {
            icon.classList.toggle('fas');
            icon.classList.toggle('far');
        }
    });
});

/**
 * Səbətə əlavə etmə funksionallığı
 * Məhsulu səbətə əlavə edir və bildiriş göstərir
 */
addToCartButtons.forEach(button => {
    button.addEventListener('click', (e) => {
        e.preventDefault();
        const productCard = button.closest('.product-card');
        const productName = productCard.querySelector('h3').textContent;
        alert(`${productName} səbətə əlavə edildi!`);
    });
});

// Filter elementlərinin seçilməsi
const filterForm = document.querySelector('.filter-group');
const genderInputs = document.querySelectorAll('input[name="gender"]');
const familyCheckboxes = document.querySelectorAll('.filter-section:nth-child(3) input[type="checkbox"]');
const brandCheckboxes = document.querySelectorAll('.filter-section:nth-child(4) input[type="checkbox"]');

/**
 * Məhsulları filter edir
 * Seçilmiş filter kriteriyalarına əsasən məhsulları göstərir və ya gizlədir
 */
function filterProducts() {
    // Seçilmiş filter dəyərlərini alır
    const selectedGenders = Array.from(document.querySelectorAll('input[name="gender"]:checked')).map(cb => cb.value);
    const selectedFamily = document.querySelector('input[name="family"]:checked')?.value || 'all';
    const selectedBrands = Array.from(document.querySelectorAll('input[name="brand"]:checked')).map(cb => cb.value);
    const minPrice = parseInt(document.querySelector('#min-price').value) || 0;
    const maxPrice = parseInt(document.querySelector('#max-price').value) || Infinity;

    const products = document.querySelectorAll('.product-card');
    
    // Hər bir məhsulu yoxlayır
    products.forEach(product => {
        const productGender = product.dataset.gender;
        const productFamily = product.dataset.family;
        const productBrand = product.dataset.brand;
        const productPrice = parseFloat(product.dataset.price);

        // Filter kriteriyalarına uyğunluğu yoxlayır
        const genderMatch = selectedGenders.includes('all') || selectedGenders.includes(productGender);
        const familyMatch = selectedFamily === 'all' || productFamily === selectedFamily;
        const brandMatch = selectedBrands.length === 0 || selectedBrands.includes(productBrand);
        const priceMatch = productPrice >= minPrice && productPrice <= maxPrice;

        // Uyğun məhsulları göstərir, uyğun olmayanları gizlədir
        if (genderMatch && familyMatch && brandMatch && priceMatch) {
            product.style.display = 'block';
            product.style.animation = 'fadeIn 0.5s ease-in-out';
        } else {
            product.style.display = 'none';
        }
    });
}

/**
 * Gender checkbox-ları üçün event listener
 * "All" seçildikdə digər seçimləri söndürür
 */
document.querySelectorAll('input[name="gender"]').forEach(checkbox => {
    checkbox.addEventListener('change', (e) => {
        if (e.target.value === 'all' && e.target.checked) {
            document.querySelectorAll('input[name="gender"]:not([value="all"])').forEach(cb => {
                cb.checked = false;
            });
        } else if (e.target.checked) {
            document.querySelector('input[name="gender"][value="all"]').checked = false;
        }
        filterProducts();
    });
});

// Digər filter elementləri üçün event listener-lar
familyCheckboxes.forEach(checkbox => {
    checkbox.addEventListener('change', filterProducts);
});

brandCheckboxes.forEach(checkbox => {
    checkbox.addEventListener('change', filterProducts);
});

minPriceInput.addEventListener('change', filterProducts);
maxPriceInput.addEventListener('change', filterProducts);
priceRange.addEventListener('input', filterProducts);

/**
 * Filter tətbiq et düyməsi
 * Filterləri tətbiq edir və paneli bağlayır
 */
const applyFiltersButton = document.querySelector('.btn-apply-filters');
applyFiltersButton.addEventListener('click', () => {
    filterProducts();
    filtersSidebar.classList.remove('active');
    document.body.style.overflow = '';
});

/**
 * Animasiya üçün CSS
 * Məhsulların fade-in effekti üçün keyframes
 */
const style = document.createElement('style');
style.textContent = `
    @keyframes fadeIn {
        from { opacity: 0; transform: translateY(20px); }
        to { opacity: 1; transform: translateY(0); }
    }
`;
document.head.appendChild(style);

/**
 * Səhifə yükləndikdə
 * İlkin filter vəziyyətini tətbiq edir
 */
document.addEventListener('DOMContentLoaded', () => {
    updatePriceInputs();
    filterProducts();
});
