# Time Tracking - Apontamento de Horas

Aplicativo em Windows Forms para controle de horas trabalhadas em projetos.

## Funcionalidades

✅ **Gerenciamento de Projetos**

- Adicionar novos projetos com nome e valor por hora
- Editar projetos existentes
- Remover projetos (todos os apontamentos associados também serão removidos)

✅ **Cronômetro**

- Iniciar/Parar cronômetro em tempo real
- Adicionar descrição do trabalho realizado
- Apontamentos manuais com data e hora customizadas

✅ **Apontamentos de Horas**

- Editar apontamentos de horas (inicio, fim e descrição)
- Remover apontamentos
- Visualizar histórico de apontamentos por projeto

✅ **Relatório Mensal**

- Visualizar total de horas trabalhadas por projeto no mês
- Calcular automaticamente o valor total (horas × taxa horária)
- Navegar entre meses

## Tecnologia

- **.NET 8.0** - Framework de desenvolvimento
- **Windows Forms** - Interface gráfica
- **SQLite** - Banco de dados local
- **sqlite-net-pcl** - ORM para SQLite

## Estrutura do Projeto

```
TimeTracking/
├── Models/
│   ├── Project.cs        - Modelo de projeto
│   └── TimeEntry.cs      - Modelo de apontamento de horas
├── Data/
│   └── DatabaseService.cs - Serviço de persistência de dados
├── Forms/
│   ├── MainForm.cs              - Tela principal com lista de projetos
│   ├── ProjectForm.cs           - Formulário para criar/editar projetos
│   ├── ProjectDetailsForm.cs    - Detalhes do projeto com cronômetro
│   └── TimeEntryForm.cs         - Formulário para criar/editar apontamentos
├── Program.cs             - Ponto de entrada da aplicação
└── TimeTracking.csproj    - Configurações do projeto
```

## Como Usar

### Compilar o Projeto

```bash
cd C:\Users\higor\source\timetracking\TimeTracking
dotnet build
```

### Executar o Aplicativo

```bash
dotnet run
```

Ou executar o arquivo gerado:

```bash
.\bin\Debug\net8.0-windows\TimeTracking.exe
```

### Usar o Aplicativo

1. **Tela Principal**
   - Clique em "+ Projeto" para adicionar um novo projeto
   - Informe o nome do projeto e o valor da hora
   - A tela mostra o total de horas e valor por projeto no mês atual
   - Use < e > para navegar entre meses

2. **Detalhes do Projeto**
   - Clique em "Detalhes" para abrir um projeto
   - Use o cronômetro: clique "Iniciar" para começar a contar o tempo
   - Adicione uma descrição do que foi feito
   - Clique "Parar" para finalizar o apontamento

3. **Apontamentos Manuais**
   - Clique "+ Manual" para adicionar um apontamento manualmente
   - Informe data/hora de início e fim
   - Adicione uma descrição

4. **Editar/Deletar**
   - Selecione um apontamento na lista
   - Clique "Editar" para modificar ou "Deletar" para remover

## Dados

Os dados são armazenados em:

```
%APPDATA%\TimeTracking\timetracking.db
```

Este é um arquivo SQLite que é criado automaticamente na primeira execução.

## Requisitos

- Windows 10/11
- .NET 8.0 Runtime (instalado automaticamente se necessário)

## Desenvolvido com

- C# 11+
- .NET 8.0
- SQLite
