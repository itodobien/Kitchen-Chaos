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
            max_tokens=300,
            temperature=0,
            system="You are a highly skilled developer with expertise in crafting clear and concise git commit messages. Analyze the provided diff and generate a commit message that effectively summarizes the changes. Start with a brief, impactful summary line (50 characters max), followed by a blank line, and then a more detailed explanation if necessary. Ensure the message is precise, informative, and adheres to best practices for commit messages.",
            messages=[
                {
                    "role": "user",
                    "content": f"Analyze the following diff and create a meaningful git commit message:\n\n{diff}"
                }
            ]
        )
        # Extract the full text from the message content
        commit_message = message.content[0].text if message.content else "Update made to the repository"
        return commit_message.strip()  # Return the full message
    except Exception as e:
        print(f"Error during API request: {e}")
        return "Update made to the repository"

def commit_changes(commit_message):
    try:
        subprocess.run(['git', 'add', '.'], check=True)
        # Use -m for the first line, and -m again for the rest of the message
        first_line, _, rest = commit_message.partition('\n')
        commit_cmd = ['git', 'commit', '-m', first_line]
        if rest:
            commit_cmd.extend(['-m', rest.strip()])
        subprocess.run(commit_cmd, check=True)
        subprocess.run(['git', 'push'], check=True)
        print("Changes committed and pushed successfully.")
        print("Full commit message:")
        print(commit_message)
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