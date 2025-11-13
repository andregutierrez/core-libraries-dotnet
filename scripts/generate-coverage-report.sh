#!/bin/bash

# Script para gerar relatório HTML de cobertura de testes
# Encontra o arquivo de cobertura mais recente e gera um relatório HTML

COVERAGE_DIR="${WORKSPACE_FOLDER:-$(pwd)}/tests/TestResults"
REPORT_DIR="${WORKSPACE_FOLDER:-$(pwd)}/tests/coverage-report"

# Encontrar o arquivo de cobertura mais recente
LATEST_COVERAGE_FILE=$(find "$COVERAGE_DIR" -name "coverage.cobertura.xml" -type f -printf '%T@ %p\n' 2>/dev/null | sort -n | tail -1 | cut -d' ' -f2-)

if [ -z "$LATEST_COVERAGE_FILE" ] || [ ! -f "$LATEST_COVERAGE_FILE" ]; then
    echo "Erro: Arquivo de cobertura não encontrado em $COVERAGE_DIR"
    echo "Execute 'test:coverage' primeiro para gerar os dados de cobertura."
    exit 1
fi

echo "Arquivo de cobertura encontrado: $LATEST_COVERAGE_FILE"

# Verificar se o ReportGenerator está instalado
if ! command -v reportgenerator &> /dev/null; then
    echo "ReportGenerator não encontrado. Instalando..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
    if [ $? -ne 0 ]; then
        echo "Erro: Falha ao instalar ReportGenerator"
        exit 1
    fi
fi

# Criar diretório de relatório se não existir
mkdir -p "$REPORT_DIR"

# Gerar relatório HTML
echo "Gerando relatório HTML..."
reportgenerator \
    "-reports:$LATEST_COVERAGE_FILE" \
    "-targetdir:$REPORT_DIR" \
    "-reporttypes:Html" \
    "-classfilters:-*Tests*" \
    "-verbosity:Info"

if [ $? -eq 0 ]; then
    REPORT_FILE="$REPORT_DIR/index.html"
    
    # Aguardar um momento para garantir que o arquivo foi completamente escrito
    sleep 0.5
    
    if [ ! -f "$REPORT_FILE" ]; then
        echo "Aviso: Arquivo de relatório não encontrado em $REPORT_FILE"
        exit 1
    fi
    
    echo ""
    echo "✓ Relatório de cobertura gerado com sucesso!"
    echo "📊 Localização: $REPORT_FILE"
    echo ""
    
    # Tentar abrir no navegador
    echo "Abrindo relatório no navegador..."
    
    # Detectar sistema operacional e usar comando apropriado
    if [[ "$OSTYPE" == "linux-gnu"* ]]; then
        # Linux
        if command -v xdg-open &> /dev/null; then
            xdg-open "$REPORT_FILE" 2>/dev/null &
        elif command -v gnome-open &> /dev/null; then
            gnome-open "$REPORT_FILE" 2>/dev/null &
        elif command -v kde-open &> /dev/null; then
            kde-open "$REPORT_FILE" 2>/dev/null &
        else
            echo "⚠ Não foi possível abrir automaticamente. Abra manualmente:"
            echo "   file://$REPORT_FILE"
        fi
    elif [[ "$OSTYPE" == "darwin"* ]]; then
        # macOS
        open "$REPORT_FILE" 2>/dev/null &
    elif [[ "$OSTYPE" == "msys" || "$OSTYPE" == "cygwin" ]]; then
        # Windows (Git Bash / Cygwin)
        if command -v start &> /dev/null; then
            start "$REPORT_FILE" 2>/dev/null &
        else
            echo "⚠ Não foi possível abrir automaticamente. Abra manualmente:"
            echo "   file://$REPORT_FILE"
        fi
    else
        echo "⚠ Sistema operacional não reconhecido. Abra manualmente:"
        echo "   file://$REPORT_FILE"
    fi
    
    # Aguardar um momento para o navegador abrir
    sleep 1
else
    echo "Erro: Falha ao gerar relatório de cobertura"
    exit 1
fi

