open LibGit2Sharp
open LibGit2Sharp.Handlers
open System
open System.IO

// args
let args = System.Environment.GetCommandLineArgs()
let (repoPath, gitdotFilePath, author, email) = (args.[1], args.[2], args.[3], args.[3])
let commitFilePath= repoPath + "\\gitdot.txt" 

// init git repo
File.Create(commitFilePath).Close()
LibGit2Sharp.Repository.Init(repoPath)  |> ignore
let repo = new Repository(repoPath)

// read data
type gitDot = { date: DateTime; commitCount : int }
let gitDots = 
    System.IO.File.ReadAllLines(gitdotFilePath) 
    |> Array.map 
        (fun line -> 
        let data = line.Split(',')
        {date = DateTime.Parse(data.[0]); commitCount = int data.[1]})
        
// collect to-commit date
let commitsDates = 
    gitDots |> Array.toList
    |> List.collect (fun gitDot -> 
        let date = gitDot.date
        let commitCount = gitDot.commitCount
        let dates = List.init commitCount (fun _ -> date)
        dates)

// generate commit
commitsDates
|> List.iter (fun date -> 
    File.AppendAllText(commitFilePath, "commits-gitdot-generated\n")
    Commands.Stage(repo, "*")
    let signature = new Signature(author, email, date)
    repo.Commit("commits-gitdot-generated", signature, signature) |> ignore
    )