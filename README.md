# gitdot
![image](https://user-images.githubusercontent.com/10369528/194772233-a1a31d0a-b1b2-4994-86f8-b1556077e07e.png)

**github as a canvas**
gitdot is a pixel art (dot image) tool for github commit activity.



## gitdot-visual
![image](https://user-images.githubusercontent.com/10369528/194772273-b92f574c-6dc8-4edc-8f14-da746289f3e3.png)
- Select year
- Cell left click : add one commit
- Cell right click : remove one commit
- save Button : Save to file for loading gitdot-cli

## gitdot-cli
A tool to create a repository with commits of the desired date
```
gitdot-cli [repo-path] [gitdot-file-path] [author] [email] [offset]
```

- repo-path : Path to be initialized by git
- gitdot-file-path : File created by gitdot-visual
- Author: committer author
- Email: Committer email
- offset : the date to be added to the commit date (for adjusting the github dashboard)

![image](https://user-images.githubusercontent.com/10369528/194773634-cf1d3f27-ff5f-4547-840c-1223b029326f.png)

After you create the repo, you can push it.
