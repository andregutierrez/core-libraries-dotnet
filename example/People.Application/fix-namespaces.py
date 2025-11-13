#!/usr/bin/env python3
import os
import re
from pathlib import Path

# Get the directory where this script is located
script_dir = Path(__file__).parent

# Patterns to replace
replacements = [
    (r'using Core\.Libraries\.Application\.Commands;', 'using Core.LibrariesApplication.Commands;'),
    (r'using Core\.Libraries\.Application\.Queries;', 'using Core.LibrariesApplication.Queries;'),
    (r'using Techleap\.Core\.Domain\.Exceptions;', 'using Core.Libraries.Domain.Exceptions;'),
]

# Find all .cs files
cs_files = list(script_dir.rglob('*.cs'))

print(f"Found {len(cs_files)} C# files to process...")

for cs_file in cs_files:
    try:
        with open(cs_file, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original_content = content
        
        # Apply all replacements
        for pattern, replacement in replacements:
            content = re.sub(pattern, replacement, content)
        
        # Only write if content changed
        if content != original_content:
            with open(cs_file, 'w', encoding='utf-8') as f:
                f.write(content)
            print(f"Fixed: {cs_file.relative_to(script_dir)}")
    except Exception as e:
        print(f"Error processing {cs_file}: {e}")

print("Done!")

