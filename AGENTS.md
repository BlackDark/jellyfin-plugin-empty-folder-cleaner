# jellyfin-plugin-empty-folder-cleaner

Jellyfin plugin that removes empty folders from media libraries.

## Verify

CI builds and tests with the pinned `jellyfin/jellyfin-meta-plugins` reusable workflows, plus actionlint, zizmor, and strict Renovate validation. The required gate is `check`.

Docs-only PRs skip build/test; `ci.yml` changes run every area. The upstream Jellyfin Renovate preset remains intentional.

## Stack

Follows https://github.com/BlackDark/agent-presets/blob/main/stack/core.md where applicable. Keep the upstream .NET and Jellyfin reusable-workflow conventions.

## Git

- Conventional Commits, one topic per PR.
- Squash unless commits need separate review.
- Force-push only with `--force-with-lease` on own branches.

## Boundaries

- Do not publish plugin packages or create release tags from a local session.
- Keep reusable workflow references pinned to full commits.
