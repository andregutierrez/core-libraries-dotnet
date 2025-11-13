# Visualização de Cobertura de Testes

Este projeto inclui ferramentas para gerar e visualizar relatórios de cobertura de testes de forma visual.

## Pré-requisitos

O projeto usa o **ReportGenerator** para converter os dados de cobertura em relatórios HTML. A ferramenta será instalada automaticamente na primeira execução, ou você pode instalá-la manualmente:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

## Como Usar

### Opção 1: Gerar Cobertura e Relatório em uma única execução

Execute a task `test:coverage:full` no VS Code (Ctrl+Shift+P → Tasks: Run Task → test:coverage:full)

Esta task irá:
1. Executar os testes com cobertura
2. Gerar automaticamente o relatório HTML
3. Abrir o relatório no navegador

### Opção 2: Gerar apenas o relatório HTML (após executar testes)

Se você já executou os testes com cobertura, pode gerar apenas o relatório:

Execute a task `test:coverage:report` no VS Code (Ctrl+Shift+P → Tasks: Run Task → test:coverage:report)

### Opção 3: Via linha de comando

```bash
# Gerar cobertura
dotnet test Core.Libraries.sln --collect:"XPlat Code Coverage" --results-directory TestResults

# Gerar relatório HTML
./scripts/generate-coverage-report.sh
```

## Localização dos Relatórios

- **Dados de cobertura (XML)**: `tests/TestResults/[guid]/coverage.cobertura.xml`
- **Relatório HTML**: `tests/coverage-report/index.html`

### Abertura Automática do Navegador

O relatório HTML será **aberto automaticamente** no seu navegador padrão após a geração. O script detecta automaticamente o sistema operacional e usa o comando apropriado:

- **Linux**: `xdg-open`, `gnome-open` ou `kde-open`
- **macOS**: `open`
- **Windows**: `start`

Se por algum motivo o navegador não abrir automaticamente, você pode abrir manualmente o arquivo `tests/coverage-report/index.html`.

## Estrutura do Relatório

O relatório HTML inclui:

- **Visão geral**: Percentual de cobertura geral (linhas, branches, complexidade)
- **Por classe**: Cobertura detalhada de cada classe
- **Por método**: Cobertura de cada método individual
- **Código fonte**: Visualização do código com linhas cobertas (verde) e não cobertas (vermelho)

## Tasks Disponíveis

- `test:coverage` - Executa testes e gera dados de cobertura (XML)
- `test:coverage:report` - Gera relatório HTML a partir dos dados existentes
- `test:coverage:full` - Executa testes, gera cobertura e relatório HTML

## Notas

- O script encontra automaticamente o arquivo de cobertura mais recente
- Classes de teste são automaticamente excluídas do relatório
- O relatório é gerado na pasta `coverage-report/` na raiz do projeto

