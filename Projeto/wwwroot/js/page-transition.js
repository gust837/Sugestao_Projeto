(function () {
    if (window.__pageTransitionReady) {
        return;
    }

    window.__pageTransitionReady = true;

    // Solução 2: o navegador, por padrão (scrollRestoration = "auto"), tenta
    // devolver o scroll para a posição antiga sempre que a URL é recarregada
    // (ex: votar/editar/excluir fazem POST -> redirect -> GET para a mesma
    // página). Se a lista ficou mais curta nesse meio tempo, o navegador
    // "clampa" o scroll no novo máximo, ou seja, no final da página.
    // Desligando a restauração automática, toda navegação nova começa do topo.
    if ('scrollRestoration' in history) {
        history.scrollRestoration = 'manual';
    }

    var transitionTime = 450;
    var playNextEnterKey = 'semcPlayPageEnter';

    function animarEntradaSePermitido() {
        if (sessionStorage.getItem(playNextEnterKey) !== 'true') {
            return;
        }

        sessionStorage.removeItem(playNextEnterKey);
        document.body.classList.add('page-entering');

        window.setTimeout(function () {
            document.body.classList.remove('page-entering');
        }, 650);
    }

    function encontrarLink(alvo) {
        if (!alvo) {
            return null;
        }

        if (alvo.closest) {
            return alvo.closest('a[href]');
        }

        if (alvo.parentElement && alvo.parentElement.closest) {
            return alvo.parentElement.closest('a[href]');
        }

        return null;
    }

    function deveIgnorarClique(event, link) {
        if (
            event.defaultPrevented ||
            event.button !== 0 ||
            event.metaKey ||
            event.ctrlKey ||
            event.shiftKey ||
            event.altKey ||
            link.target ||
            link.hasAttribute('download')
        ) {
            return true;
        }

        var url = new URL(link.href, window.location.href);
        var paginaAtual = window.location.origin + window.location.pathname;
        var proximaPagina = url.origin + url.pathname;

        return (
            url.origin !== window.location.origin ||
            link.getAttribute('href') === '#' ||
            (paginaAtual === proximaPagina && url.hash)
        );
    }

    document.addEventListener('click', function (event) {
        var link = encontrarLink(event.target);

        if (!link || deveIgnorarClique(event, link)) {
            return;
        }

        event.preventDefault();
        sessionStorage.setItem(playNextEnterKey, 'true');
        document.body.classList.add('page-leaving');

        window.setTimeout(function () {
            window.location.href = link.href;
        }, transitionTime);
    }, true);

    // Ajuste: nem todo <form> representa uma troca de página. Votar, editar
    // status e excluir, por exemplo, fazem POST e voltam pra mesma Feed —
    // não é uma "transição de página" de verdade, só uma atualização de
    // dados, então não deve disparar a animação. Por isso a transição em
    // formulário agora é opt-in: só forms marcados com o atributo
    // data-page-transition (ex: login, logout, criar post) animam.
    document.addEventListener('submit', function (event) {
        var form = event.target;

        if (!(form instanceof HTMLFormElement) || event.defaultPrevented) {
            return;
        }

        if (form.target) {
            return; // form que abre em outra aba/iframe não navega esta página
        }

        if (!form.hasAttribute('data-page-transition')) {
            return; // ação que só recarrega a página atual (votar, editar, excluir...)
        }

        sessionStorage.setItem(playNextEnterKey, 'true');
        document.body.classList.add('page-leaving');
    }, true);

    window.addEventListener('pageshow', function (event) {
        if (event.persisted) {
            document.body.classList.remove('page-leaving');
            document.body.classList.remove('page-entering');
        }
    });

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', animarEntradaSePermitido);
    } else {
        animarEntradaSePermitido();
    }
})();