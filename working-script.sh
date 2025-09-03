#!/bin/sh

# Check if SEARCH_DIR environment variable is set
if [ -z "$SEARCH_DIR" ]; then
    echo "Error: SEARCH_DIR environment variable is not set"
    exit 1
fi

# Check if the directory exists
if [ ! -d "$SEARCH_DIR" ]; then
    echo "Error: Directory '$SEARCH_DIR' does not exist"
    exit 1
fi

echo "Processing directories in: $SEARCH_DIR"

# Common video file extensions (you can modify this list as needed)
VIDEO_EXTENSIONS="avi mp4 mkv mov wmv flv webm m4v mpg mpeg ts mts m2ts 3gp 3g2 f4v"

# Function to check if directory contains video files
has_video_files() {
    local dir="$1"

    for ext in $VIDEO_EXTENSIONS; do
        # Use find to search for files with video extensions (case insensitive)
        if find "$dir" -type f \( -iname "*.$ext" \) -print -quit | grep -q .; then
            return 0  # Found video files
        fi
    done
    return 1  # No video files found
}

# Process each directory in SEARCH_DIR
for dir in "$SEARCH_DIR"/*; do
    # Skip if not a directory
    if [ ! -d "$dir" ]; then
        continue
    fi

    dir_name=$(basename "$dir")
    echo "Processing directory: $dir_name"

    if has_video_files "$dir"; then
        # Directory contains video files - remove .ignore if it exists
        if [ -f "$dir/.ignore" ]; then
            rm "$dir/.ignore"
            echo "  Removed .ignore file (video files found)"
        else
            echo "  No .ignore file to remove (video files found)"
        fi
    else
        # Directory contains no video files - create .ignore if it doesn't exist
        if [ ! -f "$dir/.ignore" ]; then
            touch "$dir/.ignore"
            echo "  Created .ignore file (no video files found)"
        else
            echo "  .ignore file already exists (no video files found)"
        fi
    fi
done

echo "Processing complete"
