// DOM Elements
const productGrid = document.getElementById('productGrid');
const productCount = document.getElementById('productCount');
const priceRange = document.getElementById('priceRange');
const minPrice = document.getElementById('minPrice');
const maxPrice = document.getElementById('maxPrice');
const sortSelect = document.getElementById('sortSelect');
const filtersClear = document.querySelector('.filters__clear');
const scrollTop = document.getElementById('scrollTop');

// State
let state = {
    filters: {
        gender: [],
        price: [50, 500],
        brands: [],
        notes: []
    },
    sort: 'popular'
};

// Debounce function for performance
const debounce = (func, wait) => {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
};

// Filter products based on current state
const filterProducts = () => {
    const products = document.querySelectorAll('.product-card');
    let visibleCount = 0;

    products.forEach(product => {
        let show = true;

        // Gender filter
        if (state.filters.gender.length) {
            const gender = product.dataset.gender;
            if (!state.filters.gender.includes(gender)) {
                show = false;
            }
        }

        // Price filter
        const price = parseInt(product.dataset.price);
        if (price < state.filters.price[0] || price > state.filters.price[1]) {
            show = false;
        }

        // Brand filter
        if (state.filters.brands.length) {
            const brand = product.dataset.brand;
            if (!state.filters.brands.includes(brand)) {
                show = false;
            }
        }

        // Notes filter
        if (state.filters.notes.length) {
            const notes = product.dataset.notes.split(',');
            if (!state.filters.notes.some(note => notes.includes(note))) {
                show = false;
            }
        }

        if (show) {
            product.classList.remove('hidden');
            visibleCount++;
        } else {
            product.classList.add('hidden');
        }
    });

    // Update product count
    productCount.textContent = visibleCount;
};

// Sort products
const sortProducts = () => {
    const products = Array.from(document.querySelectorAll('.product-card'));
    const container = productGrid;

    products.sort((a, b) => {
        const priceA = parseInt(a.dataset.price);
        const priceB = parseInt(b.dataset.price);

        switch (state.sort) {
            case 'price-asc':
                return priceA - priceB;
            case 'price-desc':
                return priceB - priceA;
            case 'new':
                return b.dataset.id - a.dataset.id;
            default:
                return 0;
        }
    });

    products.forEach(product => {
        container.appendChild(product);
    });
};

// Event Listeners
document.querySelectorAll('input[name="gender"]').forEach(checkbox => {
    checkbox.addEventListener('change', (e) => {
        if (e.target.checked) {
            state.filters.gender.push(e.target.value);
        } else {
            state.filters.gender = state.filters.gender.filter(g => g !== e.target.value);
        }
        filterProducts();
    });
});

document.querySelectorAll('input[name="brand"]').forEach(checkbox => {
    checkbox.addEventListener('change', (e) => {
        if (e.target.checked) {
            state.filters.brands.push(e.target.value);
        } else {
            state.filters.brands = state.filters.brands.filter(b => b !== e.target.value);
        }
        filterProducts();
    });
});

priceRange.addEventListener('input', debounce((e) => {
    state.filters.price[1] = parseInt(e.target.value);
    maxPrice.textContent = `${e.target.value}₼`;
    filterProducts();
}, 300));

document.querySelectorAll('input[name="notes"]').forEach(checkbox => {
    checkbox.addEventListener('change', (e) => {
        if (e.target.checked) {
            state.filters.notes.push(e.target.value);
        } else {
            state.filters.notes = state.filters.notes.filter(n => n !== e.target.value);
        }
        filterProducts();
    });
});

sortSelect.addEventListener('change', (e) => {
    state.sort = e.target.value;
    sortProducts();
});

filtersClear.addEventListener('click', () => {
    state.filters = {
        gender: [],
        price: [50, 500],
        brands: [],
        notes: []
    };
    state.sort = 'popular';
    
    // Reset UI
    document.querySelectorAll('input[type="checkbox"]').forEach(checkbox => checkbox.checked = false);
    priceRange.value = 500;
    maxPrice.textContent = '500₼';
    sortSelect.value = 'popular';
    
    filterProducts();
});

// Scroll to top functionality
window.addEventListener('scroll', () => {
    if (window.pageYOffset > 300) {
        scrollTop.classList.add('visible');
    } else {
        scrollTop.classList.remove('visible');
    }
});

scrollTop.addEventListener('click', () => {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
});

// Initialize
const init = () => {
    filterProducts();
    sortProducts();
};

// Start the application
init(); 