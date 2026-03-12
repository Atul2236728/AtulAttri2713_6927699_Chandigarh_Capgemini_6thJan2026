function addTask()
{
let task=document.getElementById("taskInput").value

let li=document.createElement("li")

li.innerHTML=task

li.onclick=function()
{
li.classList.toggle("completed")
}

let del=document.createElement("button")
del.innerHTML="Delete"

del.onclick=function()
{
li.remove()
}

li.appendChild(del)

document.getElementById("taskList").appendChild(li)
}