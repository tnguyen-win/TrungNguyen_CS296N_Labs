// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const eMobiles = document.getElementsByClassName('eMobiles');
const eMobileButton = document.getElementById('eMobileButton');

function toggleMobile() {
    Array.from(eMobiles).forEach(e => e.classList.contains('hidden') ? (e.classList.remove('hidden'), eMobileButton.innerText = 'b') : (e.classList.add('hidden'), eMobileButton.innerText = 'a'));
}

function resetMobile() {
    Array.from(eMobiles).forEach(e => window.matchMedia('(min-width: 1024px)') ? e.classList.add('hidden') : {});
}

function dismissCookiesBanner(e) {
    e.parentElement.parentElement.classList.add('hidden');
}

function checkScroll() {
    const eBtn = document.getElementById('buttonScrollToTop');
    let classes = [];

    classes = document.body.scrollTop > 20 || document.documentElement.scrollTop > 20 ? ['invisible', 'visible'] : ['visible', 'invisible'];

    eBtn.classList.replace(classes[0], classes[1]);
}

function scrollToTop() {
    document.body.scroll({
        top: 0,
        left: 0,
        behavior: 'smooth'
    });
    document.documentElement.scroll({
        top: 0,
        left: 0,
        behavior: 'smooth'
    });
}

window.addEventListener('load', checkScroll);
window.addEventListener('resize', resetMobile);
window.addEventListener('scroll', checkScroll);
window.addEventListener('touchmove', checkScroll);
