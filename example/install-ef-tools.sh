#!/bin/bash

# Script to install EF Core tools
# Usage: ./install-ef-tools.sh

set -e

echo "=========================================="
echo "Installing EF Core Tools"
echo "=========================================="

# Check if dotnet-ef is already installed
if dotnet ef --version > /dev/null 2>&1; then
    echo "✓ dotnet-ef is already installed"
    dotnet ef --version
    exit 0
fi

echo "Installing dotnet-ef tool globally..."
dotnet tool install --global dotnet-ef --version 9.0.0

# Add dotnet tools to PATH if not already there
if [[ ":$PATH:" != *":$HOME/.dotnet/tools:"* ]]; then
    echo ""
    echo "⚠️  WARNING: dotnet tools directory is not in your PATH"
    echo "Add this line to your ~/.bashrc or ~/.zshrc:"
    echo "export PATH=\"\$PATH:\$HOME/.dotnet/tools\""
    echo ""
    echo "Or run this command now:"
    echo "export PATH=\"\$PATH:\$HOME/.dotnet/tools\""
    echo ""
fi

echo ""
echo "=========================================="
echo "Installation completed!"
echo "=========================================="
echo ""
echo "To verify installation, run:"
echo "  dotnet ef --version"
echo ""
echo "If you get 'command not found', add to your PATH:"
echo "  export PATH=\"\$PATH:\$HOME/.dotnet/tools\""
echo ""

