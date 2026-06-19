function esconder(btn) {
    var card = btn.closest('.post-card');

    var elementos = [
        card.querySelector('.status-panel'),
        card.querySelector('.salvar-btn'),
        card.querySelector('.excluir-btn')
    ];

    elementos.forEach(function(el) {
        if (el) {
            el.style.display = (el.style.display === 'none') ? 'block' : 'none';
        }
    });
}