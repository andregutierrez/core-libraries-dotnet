#!/usr/bin/env python3
import os
import re
import sys

def fix_file(filepath):
    try:
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()
        
        original = content
        
        # Apply replacements
        content = re.sub(r'using Core\.Libraries\.Application\.Commands;', 'using Core.LibrariesApplication.Commands;', content)
        content = re.sub(r'using Core\.Libraries\.Application\.Queries;', 'using Core.LibrariesApplication.Queries;', content)
        content = re.sub(r'using Techleap\.Core\.Domain\.Exceptions;', 'using Core.Libraries.Domain.Exceptions;', content)
        
        if content != original:
            with open(filepath, 'w', encoding='utf-8') as f:
                f.write(content)
            return True
    except Exception as e:
        print(f"Error processing {filepath}: {e}", file=sys.stderr)
    return False

if __name__ == '__main__':
    fixed = 0
    for root, dirs, files in os.walk('.'):
        for file in files:
            if file.endswith('.cs'):
                filepath = os.path.join(root, file)
                if fix_file(filepath):
                    fixed += 1
    print(f'Fixed {fixed} files')

