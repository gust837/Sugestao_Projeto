/* Função de esconder a senha */
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
/* Modais de confirmação de exclusçao e salvar alterações */
function confirmarStatus(id) {
    var select = document.getElementById('select-status-' + id);

    if (!select || !select.value) {
        Swal.fire({
            title: 'Atenção',
            text: 'Selecione um status antes de confirmar.',
            icon: 'warning',
            background: 'hsl(0, 0%, 8%)',
            color: 'hsl(0, 0%, 95%)',
            confirmButtonColor: 'rgb(104, 44, 201)'
        });
        return;
    }

    Swal.fire({
        title: 'Confirmar alteração?',
        text: 'O status dessa sugestão será atualizado.',
        icon: 'question',
        background: 'hsl(0, 0%, 8%)',
        color: 'hsl(0, 0%, 95%)',
        showCancelButton: true,
        confirmButtonColor: 'rgb(104, 44, 201)',
        cancelButtonColor: 'hsl(0, 0%, 30%)',
        confirmButtonText: 'Sim, confirmar!',
        cancelButtonText: 'Cancelar'
    }).then(function (result) {
        if (result.isConfirmed) {
            document.getElementById('form-editar-' + id).submit();
        }
    });
}

function confirmarExclusao(id) {
    Swal.fire({
        title: 'Tem certeza?',
        text: 'Esta sugestão será excluída permanentemente e não pode ser desfeita!',
        icon: 'warning',
        background: 'hsl(0, 0%, 8%)',
        color: 'hsl(0, 0%, 95%)',
        showCancelButton: true,
        confirmButtonColor: '#ef4444',
        cancelButtonColor: 'hsl(0, 0%, 30%)',
        confirmButtonText: 'Sim, excluir!',
        cancelButtonText: 'Cancelar'
    }).then(function (result) {
        if (result.isConfirmed) {
            document.getElementById('form-excluir-' + id).submit();
        }
    });
}