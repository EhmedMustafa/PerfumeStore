// Breadcrumb naviqasiyasını dinamik etmək
document.addEventListener('DOMContentLoaded', function() {
    const breadcrumbList = document.querySelector('.breadcrumb-list');
    
    if (breadcrumbList) {
        const currentPath = window.location.pathname;
        const pathParts = currentPath.split('/');
        const brandName = decodeURIComponent(pathParts[pathParts.length - 1].replace('.html', ''));
        
        const breadcrumbData = {
            'Acqua di Parma': 'Acqua di Parma',
            'Chanel': 'Chanel',
            'Dior': 'Dior',
            'Gucci': 'Gucci'
        };
        
        const currentPageName = breadcrumbData[brandName] || brandName;
        
        breadcrumbList.innerHTML = `
            <li><a href="/">Ana səhifə</a></li>
            
            <li><span>/</span></li>
            <li>${currentPageName}</li>
        `;
    }
}); 