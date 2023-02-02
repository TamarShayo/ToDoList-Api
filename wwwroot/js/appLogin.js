const url = "/MyUser";
var myToken = "";

const Login = () => {
  let userName = document.getElementById("Name");
  let userPassword = document.getElementById("Password");

  const raw = JSON.stringify({
    Name: userName.value.trim(),
    Password: userPassword.value.trim(),
  });

  var myHeaders = new Headers();
  myHeaders.append("Content-Type", "application/json");

  var requestOptions = {
    method: "POST",
    headers: myHeaders,
    body: raw,
    redirect: "follow",
  };
  
  fetch(`${url}/Login`, requestOptions)
    .then((response) => response.text())
    .then((result) => {
      if (result.includes("401")) {
        alert(userName.value + " is not logged in");
        userName.value = "";
        userPassword.value = "";
      } else {
        sessionStorage.setItem("token", result);
        location.href = "/list.html";
      }
    })
    .catch((error) => alert("error"));
};

displayUsers(result);
