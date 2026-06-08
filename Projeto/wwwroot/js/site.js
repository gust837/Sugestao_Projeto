const botoesExcluir = document.querySelectorAll('.btn_excluir_livro');
botoesExcluir.forEach(btn => {
    btn.addEventListener('click', () => {
        const id = btn.getAttribute('data-id');

        Swal.fire({
            title: 'Tem certeza?',
            text: "Esta ação não pode ser desfeita!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sim, excluir!',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(`/Livro/Excluir/${id}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Excluído!',
                            'O livro foi excluído com sucesso.',
                            'success'
                        ).then(() => {
                            window.location.reload();
                        });
                    } else {
                        Swal.fire(
                            'Erro!',
                            'Ocorreu um problema ao tentar excluir o livro.',
                            'error'
                        );
                    }
                }).catch(error => {
                    Swal.fire('Erro!', 'Não foi possível completar a operação.', 'error');
                });
            }
        });
    });
});
