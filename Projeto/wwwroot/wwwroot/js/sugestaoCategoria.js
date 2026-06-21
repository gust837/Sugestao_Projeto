// -------------------- LÓGICA DE CATEGORIAS --------------------
const selectCategoria = document.getElementById("categoria");
const botaoInserir = document.getElementById("cad_btn_inserir");
const divListaCats = document.getElementById("cad_lista_cats");
const inputCategoriasSelecionadas = document.getElementById("CategoriasSelecionadas");

let catsSelecionadas = [];

function adicionarCat() {
    const valorSelecionado = selectCategoria.value;
    const nomeSelecionado = selectCategoria.options[selectCategoria.selectedIndex].text;

    if (valorSelecionado === "") {
        alert("Selecione uma categoria");
        return;
    }

    if (catsSelecionadas.find(c => c.id === valorSelecionado)) {
        alert("Essa categoria já foi selecionada");
        return;
    }

    catsSelecionadas.push({ id: valorSelecionado, nome: nomeSelecionado });
    selectCategoria.value = "";
    renderizarItens();
}

function renderizarItens() {
    let template = "";

    if (catsSelecionadas.length > 0) {
        catsSelecionadas.forEach(cat => {
            template += `
            <div class="cad_item_cat">
                <span class="cad_span_cat">${cat.nome}</span>
                <button type="button" class="btn_item_excluir" onclick="excluirCat('${cat.id}')">
                    <svg xmlns="http://www.w3.org/2000/svg" height="1em" viewBox="0 0 384 512">
                        <path fill="currentColor" d="M342.6 150.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L192 210.7 86.6 105.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L146.7 256 41.4 361.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L192 301.3 297.4 406.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L237.3 256 342.6 150.6z"/>
                    </svg>
                </button>
            </div>`;
        });
    } else {
        template = `<span class="cad_span_cat_vazio">Nenhuma categoria selecionada</span>`;
    }

    divListaCats.innerHTML = template;
    inputCategoriasSelecionadas.value = catsSelecionadas.map(c => c.id).join(',');
}

function excluirCat(id) {
    catsSelecionadas = catsSelecionadas.filter(item => item.id !== id);
    renderizarItens();
}

if(botaoInserir) {
    botaoInserir.addEventListener("click", adicionarCat);
    renderizarItens();
}