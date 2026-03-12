function register()
{
let user=document.getElementById("regUser").value
let email=document.getElementById("regEmail").value
let pass=document.getElementById("regPass").value
let confirm=document.getElementById("regConfirm").value

if(!email.includes("@"))
{
alert("Invalid Email")
return
}

if(pass.length<6)
{
alert("Password must be 6 characters")
return
}

if(pass!==confirm)
{
alert("Passwords do not match")
return
}

let userData={
username:user,
email:email,
password:pass
}

localStorage.setItem("user",JSON.stringify(userData))

alert("Registration Successful")
}

function login()
{
let user=document.getElementById("loginUser").value
let pass=document.getElementById("loginPass").value

let stored=JSON.parse(localStorage.getItem("user"))

if(stored && stored.username===user && stored.password===pass)
{
alert("Login Successful")
}
else
{
alert("Invalid Credentials")
}
}