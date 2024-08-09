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
    client = anthropic.Anthropic(api_key=ANTHROPIC_API_KEY)
   
    try:
        message = client.messages.create(
            model="claude-3-5-sonnet-20240620",
            max_tokens=100,
            temperature=0,
            system="You are an expert in creating concise and meaningful git commit messages. Analyze the provided diff and create a commit message.",
            messages=[
                {
                    "role": "user",
                    "content": f"Analyze the following diff and create a concise and meaningful git commit message:\n\n{diff}"
                }
            ]
        )
        # Extract the text from the message content
        commit_message = message.content[0].text if message.content else "Update made to the repository"
        return commit_message.split('\n')[0]  # Return only the first line
    except Exception as e:
        print(f"Error during API request: {e}")
        return "Update made to the repository"

def commit_changes(commit_message):
    try:
        subprocess.run(['git', 'add', '.'], check=True)
        # Use -m for each line of the commit message
        commit_cmd = ['git', 'commit']
        for line in commit_message.split('\n'):
            commit_cmd.extend(['-m', line])
        subprocess.run(commit_cmd, check=True)
        subprocess.run(['git', 'push'], check=True)
        print("Changes committed and pushed successfully.")
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