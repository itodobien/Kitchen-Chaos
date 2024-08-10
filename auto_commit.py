import os
import subprocess
import anthropic
from dotenv import load_dotenv

# Load environment variables
load_dotenv()

# Use the API key from the environment variable
ANTHROPIC_API_KEY = os.getenv("ANTHROPIC_API_KEY")

def is_git_repository():
    try:
        subprocess.run(['git', 'rev-parse', '--is-inside-work-tree'], check=True, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
        return True
    except subprocess.CalledProcessError:
        return False

def get_git_changes():
    try:
        result = subprocess.run(['git', 'status', '--porcelain'], check=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
        changed_files = result.stdout.decode('utf-8').strip()
        if not changed_files:
            return None, None
        result = subprocess.run(['git', 'diff'], check=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
        diff = result.stdout.decode('utf-8').strip()
        return changed_files, diff
    except subprocess.CalledProcessError as e:
        print(f"Error getting git changes: {e}")
        return None, None

def generate_commit_message(diff):
    client = anthropic.Anthropic()
    
    try:
        message = client.messages.create(
            model="claude-3-5-sonnet-20240620",
            max_tokens=1000,
            temperature=0,
            system="You are an expert in creating concise and meaningful git commit messages. Analyze the provided diff and create a commit message that summarizes the changes effectively.",
            messages=[
                {
                    "role": "user",
                    "content": f"Analyze this diff and create a git commit message:\n\n{diff}"
                }
            ]
        )
        commit_message = message.content[0].text if message.content else "Update repository"
        return commit_message.strip()
    except Exception as e:
        print(f"Error during API request: {e}")
        return "Update repository"

def commit_changes(commit_message):
    try:
        subprocess.run(['git', 'add', '.'], check=True)
        first_line, _, rest = commit_message.partition('\n')
        commit_cmd = ['git', 'commit', '-m', first_line]
        if rest:
            commit_cmd.extend(['-m', rest.strip()])
        subprocess.run(commit_cmd, check=True)
        subprocess.run(['git', 'push'], check=True)
        print("Changes committed and pushed successfully.")
        print("Commit message:", commit_message)
    except subprocess.CalledProcessError as e:
        print(f"Error committing changes: {e}")

def main():
    if not is_git_repository():
        print("Current directory is not a Git repository.")
        return
    if not ANTHROPIC_API_KEY:
        print("ANTHROPIC_API_KEY is not set in the environment variables.")
        return
    changed_files, diff = get_git_changes()
    if not diff:
        print("No changes detected.")
        return
    print("Changes detected:", changed_files)
    print("Generating commit message...")
    commit_message = generate_commit_message(diff)
    print("Committing changes...")
    commit_changes(commit_message)

if __name__ == "__main__":
    main()