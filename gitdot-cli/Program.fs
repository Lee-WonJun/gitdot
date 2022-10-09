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
type LineData = { Date: DateTime; CommitCount : int }
let lineDatas = 
    System.IO.File.ReadAllLines(gitdotFilePath) |> Array.toList
    |> List.map (fun line -> line.Split(","))
    |> List.map (fun arr -> { Date = DateTime.Parse(arr.[0]); CommitCount = int arr.[1] })
        
// collect to-commit date
let signatures = 
    lineDatas
    |> List.collect (fun gitDot -> 
        let { Date= date; CommitCount = commitCount} = {gitDot with Date = gitDot.Date.AddDays(offset)}
        let signature = new Signature(author, email, date)
        List.init commitCount (fun _ -> signature))
    
// generate commit
signatures
|> List.iter (fun signature -> 
    File.AppendAllText(commitFilePath, "commits-gitdot-generated\n")
    Commands.Stage(repo, "*")
    repo.Commit("commits-gitdot-generated", signature, signature) |> ignore)