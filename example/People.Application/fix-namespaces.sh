#!/bin/bash

# Script to fix namespace issues in People.Application
cd "$(dirname "$0")"

# Fix Core.Libraries.Application.Commands -> Core.LibrariesApplication.Commands
find . -name "*.cs" -type f -exec sed -i 's/using Core\.Libraries\.Application\.Commands;/using Core.LibrariesApplication.Commands;/g' {} \;

# Fix Core.Libraries.Application.Queries -> Core.LibrariesApplication.Queries
find . -name "*.cs" -type f -exec sed -i 's/using Core\.Libraries\.Application\.Queries;/using Core.LibrariesApplication.Queries;/g' {} \;

# Fix Core.LibrariesDomain.Exceptions -> Core.Libraries.Domain.Exceptions
find . -name "*.cs" -type f -exec sed -i 's/using Techleap\.Core\.Domain\.Exceptions;/using Core.Libraries.Domain.Exceptions;/g' {} \;

echo "Namespace fixes applied!"

