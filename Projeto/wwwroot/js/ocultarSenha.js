/* Função de mostrar a senha */
function mostrarSenha(){
    var inputPass = document.getElementById('senha')
    var btnShowPass = document.getElementById('btn_senha')

    if (inputPass.type === 'password') 
    {
        inputPass.setAttribute('type', 'text')
        btnShowPass.classList.replace('ri-eye-line', 'ri-eye-off-line')
    }

    else
    {
        inputPass.setAttribute('type', 'password')
        btnShowPass.classList.replace('ri-eye-off-line', 'ri-eye-line')
    }
} 