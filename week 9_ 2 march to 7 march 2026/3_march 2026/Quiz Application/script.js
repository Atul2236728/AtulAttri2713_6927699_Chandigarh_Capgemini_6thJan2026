let questions=[
{
q:"Capital of India?",
options:["Delhi","Mumbai","Kolkata","Chennai"],
answer:0
},

{
q:"2+2=?",
options:["2","3","4","5"],
answer:2
}
]

let index=0
let score=0

function loadQuestion()
{
document.getElementById("question").innerText=questions[index].q

for(let i=0;i<4;i++)
{
document.getElementById("opt"+i).innerText=questions[index].options[i]
}
}

function check(option)
{
if(option===questions[index].answer)
score++

index++

if(index<questions.length)
loadQuestion()
else
document.getElementById("score").innerText="Score: "+score
}

loadQuestion()