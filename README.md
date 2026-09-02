<h1 align="center"> ADVANCED BUSINESS DEVELOPMENT WITH .NET </h1>
<h3 align="center">Challenge - FIAP - CYLVO</h3>

<h2 align="center" >  👥 Integrante </h2>

- Eduardo Batista Locaspi | RM565171 
- Liana Lyumi Morisita Fujisima | RM565698 
- Leticia Santiago e Silva | RM565799 
- Victor Alves Lopes | RM561833 

<h2> 📋 Descrição sobre projeto: </h2>

<p>
	A **CLYVO** nasceu para resolver um problema real e silencioso: a descontinuidade no cuidado com a saúde dos pets.

Como os animais não conseguem comunicar o que sentem, é comum que os tutores só os levem ao veterinário em situações de emergência ou quando o quadro já está grave. Isso resulta em:
</p>

- ❌ Ausência de histórico clínico do animal
- ❌ Diagnósticos tardios e tratamentos mais complexos
- ❌ Baixa frequência de check-ups preventivos

<h2> 💡 A Solução</h2>

<p>A CLYVO entrega um **aplicativo completo para veterinários**, reunindo em um único lugar tudo o que é necessário para acompanhar a jornada de saúde dos pacientes:
</p>

- 🐶 Dados do animal (idade, peso, espécie, raça)
- 💉 Carteira de vacinação
- 📋 Histórico de consultas e prescrições
- 👤 Informações do responsável
- 📍 Localização do pet (caso more em endereço diferente do tutor)
- 📅 Agenda de próximas consultas com alertas de cancelamento ou alteração

<h2> 📲 Engajamento via WhatsApp </h2>

<p>Para aumentar a recorrência de visitas, o sistema envia mensagens automáticas ao responsável pelo animal após um determinado período desde a última consulta ou vacinação, perguntando sobre o interesse em agendar um novo atendimento — com opção de confirmar ou recusar diretamente pela mensagem.</p>

<h2>⚙️ Sobre o Backend (.NET) </h2>

<p>O projeto .NET atua como o **sistema de backend CRUD** da plataforma, responsável por:</p>

- Gerenciar todos os dados da aplicação no Banco de Dados Oracle
- Expor APIs REST para que o Frontend consuma e interaja com os dados


<h2> 🛣️ Rotas da API: </h2>

Rota base: `api/[controller]`

<h4> 🐾 Animal</h4>

- GET - All: relatorio/animal
- GET - ID: relatorio/animal/{id_animal:int}
- POST: criar/animal
- PUT: atualizar/animal/{id_animal:int}
- DELETE: deleta/animal/{id_animal:int}

<h4>🩺 Veterinário</h4>

- GET - ALL: relatorio/veterinario
- GET - ID: relatorio/veterinario/{id_vet:int}
- POST: criar/veterinario
- PUT: atualizar/veterinario/{id_vet:int}
- DELETE: deleta/veterinario/{id_vet:int}

<h4>👤 Responsável</h4>

- GET - ALL: relatorio/responsavel
- GET - ID: relatorio/responsavel/{id_responsavel:int}
- POST: criar/responsavel
- PUT: atualizar/responsavel/{id_responsavel:int}
- DELETE: deleta/responsavel/{id_responsavel:int}

<h4> 📅 Consulta</h4>

- GET - ALL: relatorio/consulta
- GET - ID: relatorio/consulta/{id_consulta:int}
- POST: criar/consulta
- PUT: atualizar/consulta/{id_consulta:int}
- DELETE: deleta/consulta/{id_consulta:int}

<h4> 📝 Prescrição</h4>

- GET - ALL: relatorio/prescricao
- GET - ID: relatorio/prescricao/{id_prescricao:int}
- POST: criar/prescricao
- PUT: atualizar/prescricao/{id_prescricao:int}
- DELETE: deleta/prescricao/{id_prescricao:int}


<h4> 💊 Medicamento</h4>

- GET - All: relatorio/medicamento
- GET - ID: relatorio/medicamento/{id_medicamento:int}
- POST: criar/medicamento
- PUT: atualizar/medicamento/{id_medicamento:int}
- DELETE: deleta/medicamento/{id_medicamento:int}

<h4> 📍 Endereço Animal</h4>

- GET - ALL: relatorio/enderecoanimal
- GET - ID: relatorio/enderecoanimal/{id_endereco_animal:int}
- PUT: atualizar/enderecoanimal/{id_endereco_animal:int}
- POST: criar/enderecoanimal
- DELETE: deleta/enderecoanimal/{id_endereco_animal:int}

<h4> 🏥 Endereço Clínica</h4>

- GET - ALL: relatorio/enderecoclinica
- GET - ID: relatorio/enderecoclinica/{id_endereco_clinica:int}
- POST: criar/enderecoclinica
- PUT: atualizar/enderecoclinica/{id_endereco_clinica:int}
- DELETE: deleta/enderecoclinica/{id_endereco_clinica:int}

<h4> 🏠 Endereço Responsável</h4>

- GET - ALL: relatorio/enderecoresponsavel
- GET - ID: relatorio/enderecoresponsavel/{id_endereco_responsavel:int}
- POST: criar/enderecoresponsavel
- PUT: atualizar/enderecoresponsavel/{id_endereco_responsavel:int}
- DELETE: deleta/enderecoresponsavel/{id_endereco_responsavel:int}

<h4> 🏢 VetClinica</h4>

- GET - ALL: relatorio/vetclinica
- GET - ID: relatorio/vetclinica/{id_clinica_vet:int}
- POST: criar/vetclinica
- PUT: atualizar/vetclinica/{id_clinica_vet:int}
- DELETE: deleta/vetclinica/{id_clinica_vet:int}

<h2>🚀 Instruções de instalação e execução: </h2>

<h4>Pre-requisitos: </h4>

- [.NET SDK 10.0 (10.0.300)](https://dotnet.microsoft.com/download)
- Oracle Database
- Pacote NuGet: `Microsoft.EntityFrameworkCore.Tools`

<h4> 1. Clonar o Repositório </h4>

- git clone https://github.com/santiago-leticia/challenge_fiap_net.git

<h4>2. Abrir o Projeto</h4>

Abra o arquivo de solução no Visual Studio ou Rider:

```
challengeFiap.slnx
```

<h4>3. Restaurar Dependências </h4>


```bash
dotnet restore
```


<h4>Para configurar o Banco de Dados - Oracle</h4>


<h4>4. Configurar o Banco de Dados </h4>

No arquivo `appsettings.json`, configure a string de conexão Oracle:

```json
"OracleConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=SEU_SERVIDOR"
```
<h4>5. Aplicar as Migrations</h4>

```bash
dotnet ef database update
```

<h4>6. Executar o Projeto </h4>

Via Visual Studio — clique em **▶ Play (http)**

Ou via terminal:

```bash
dotnet run
```

<h4> 7. Acessar o Swagger</h4>

Após iniciar, o terminal exibirá a porta utilizada. Acesse a documentação interativa da API em:

```
https://localhost:{PORTA}/swagger
```
> Substitua `{PORTA}` pelo número exibido no terminal ao iniciar a aplicação.

---
<h4>🛠️ Tecnologias Utilizadas</h4>

- **C# / .NET 10**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **Oracle Database**
- **Swagger / OpenAPI**

---
*Desenvolvido com 💙 pela equipe CLYVO — FIAP 2026*



