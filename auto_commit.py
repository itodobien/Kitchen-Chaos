import os
import subprocess
import requests
from dotenv import load_dotenv

# Load environment variables
load_dotenv()

# Define your Claude API endpoint and API key
CLAUDE_API_URL = "https://api.anthropic.com/v1/claude"
CLAUDE_API_KEY = os.getenv("CLAUDE_API_KEY")

def is_git_repository():
    try: 
        subprocess.run(['git', 'rev-parse', '--is-inside-work-tree'], check=True, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
        return True
    except subprocess.CalledProcessError:
        return False

def get_git_changes():
    try:
        # Get the list of changed files
        result = subprocess.run(['git', 'status', '--porcelain'], check=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
        changed_files = result.stdout.decode('utf-8').strip()
        if not changed_files:
            return None, None
        # Get the diff of the changes
        result = subprocess.run(['git', 'diff'], check=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
        diff = result.stdout.decode('utf-8').strip()
        return changed_files, diff
    except subprocess.CalledProcessError as e:
        print(f"Error getting git changes: {e}")
        return None, None

def generate_commit_message(diff):
    headers = {
        "Authorization": f"Bearer {CLAUDE_API_KEY}",
        "Content-Type": "application/json"
    }
    payload = {
        "prompt": f"Analyze the following diff and create a concise and meaningful git commit message:\n\n{diff}",
        "max_tokens": 100,
        "model": "claude-v1"
    }
    try:
        response = requests.post(CLAUDE_API_URL, json=payload, headers=headers)
        response.raise_for_status()
        response_data = response.json()
        if "choices" in response_data and response_data["choices"]:
            message = response_data["choices"][0].get("text", "").strip()
            return message if message else "Update made to the repository"
        else:
            print("Unexpected response format or empty 'choices'. Response data:", response_data)
            return "Update made to the repository"
    except requests.exceptions.RequestException as e:
        print("Error during API request:", e)
        return "Update made to the repository"

def commit_changes(commit_message):
    try:
        subprocess.run(['git', 'add', '.'], check=True)
        subprocess.run(['git', 'commit', '-m', commit_message], check=True)
        subprocess.run(['git', 'push'], check=True)
        print("Changes committed and pushed successfully.")
    except subprocess.CalledProcessError as e:
        print(f"Error committing changes: {e}")

def main():
    if not is_git_repository():
        print("Current directory is not a Git repository.")
        return

    if not CLAUDE_API_KEY:
        print("CLAUDE_API_KEY is not set in the environment variables.")
        return

    changed_files, diff = get_git_changes()
    if not diff:
        print("No changes detected.")
        return

    print("Changes detected:")
    print(changed_files)
    print("Generating commit message...")
    commit_message = generate_commit_message(diff)
    print("Generated commit message:")
    print(commit_message)
    print("Committing changes...")
    commit_changes(commit_message)

if __name__ == "__main__":
    main()