document.documentElement.classList.add('js');

document.addEventListener('DOMContentLoaded', function () {
    var alvos = document.querySelectorAll('.how-it-works, .about__inner, .card, .footer');
    var jaRolou = window.scrollY > 80;
    var pendentes = new Set();

    function mostrarElemento(el) {
        el.classList.add('is-visible');
        observer.unobserve(el);
        pendentes.delete(el);
    }

    function elementoEntrouNaTela(el) {
        var posicao = el.getBoundingClientRect();
        return posicao.top < window.innerHeight * 0.9 && posicao.bottom > 0;
    }

    function mostrarPendentesAoRolar() {
        if (!jaRolou && window.scrollY > 80) {
            jaRolou = true;
        }

        alvos.forEach(function (el) {
            if (
                !el.classList.contains('is-visible') &&
                !el.classList.contains('card') &&
                elementoEntrouNaTela(el)
            ) {
                mostrarElemento(el);
            }
        });

        if (jaRolou) {
            pendentes.forEach(function (el) {
                mostrarElemento(el);
            });
        }
    }

    var observer = new IntersectionObserver(function (entradas) {
        entradas.forEach(function (entrada) {
            if (!entrada.isIntersecting) {
                return;
            }

            if (entrada.target.classList.contains('card') && !jaRolou) {
                pendentes.add(entrada.target);
                return;
            }

            mostrarElemento(entrada.target);
        });
    }, {
        rootMargin: '0px 0px -8% 0px',
        threshold: 0.05
    });

    alvos.forEach(function (el) {
        observer.observe(el);
    });

    window.addEventListener('scroll', mostrarPendentesAoRolar, { passive: true });
    mostrarPendentesAoRolar();
});
