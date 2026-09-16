#!/usr/bin/env bash
set -euo pipefail

# The optional compatibility libraries affect this process and its children only.
repo_dir="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")/.." && pwd)"
editor="${UNITY_EDITOR:-$HOME/Unity/Hub/Editor/6000.6.1f1/Editor/Unity}"
compat_dir="$repo_dir/.unity-compat/usr/lib/x86_64-linux-gnu"

if [[ -d "$compat_dir" ]]; then
    export LD_LIBRARY_PATH="$compat_dir${LD_LIBRARY_PATH:+:$LD_LIBRARY_PATH}"
fi

exec "$editor" -projectPath "$repo_dir/UnityProject" "$@"
