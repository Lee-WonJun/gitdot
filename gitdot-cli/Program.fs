open LibGit2Sharp
open LibGit2Sharp.Handlers
open System
open System.IO
open System.Globalization

// args
let args = System.Environment.GetCommandLineArgs()
let (repoPath, gitdotFilePath, author, email) = (args.[1], args.[2], args.[3], args.[4])
let offset = Int32.Parse(args |> Array.tryItem 5 |> Option.defaultValue "1") // In korea, we need to add 1 day.. I don't know why...
let commitFilePath= repoPath + "\\gitdot.txt" 

// init git repo
File.Create(commitFilePath).Close()
LibGit2Sharp.Repository.Init(repoPath)  |> ignore
let repo = new Repository(repoPath)

// print args
printfn "Repository : %s" repoPath
printfn "git-dot file : %s" gitdotFilePath
printfn "author : %s / email :%s" author email

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
        let date = gitDot.date.AddDays(offset )
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