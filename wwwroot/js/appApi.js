const url = "/MyUser";
let tasks = [];
let Token = sessionStorage.getItem("token");

// --------------- User ---------------
getUsers();
function getUsers() {
  var myHeaders = new Headers();
  myHeaders.append("Authorization", `Bearer ${Token}`);
  myHeaders.append("Content-Type", "application/json");
  var requestOptions = {
    method: "GET",
    headers: myHeaders,
    redirect: "follow",
  };

  fetch(`${url}/GetAll`, requestOptions)
    .then((response) => response.json())
    .then((data) => {
      const button = document.getElementById("userBlock");
      button.style.display = "block";
      displayUsers(data);
    })
    .catch((error) => console.log("error", error));
}

function displayUsers(data) {
  const tBody = document.getElementById("result");
  tBody.innerHTML = "";

  _displayCountUser(data.length);
  var button = document.createElement("button");
  button.classList.add("TransparentButton");
  data.forEach((user) => {
    console.log(user.name);

    let isAdminCheckbox = document.createTextNode(isAdminStatus());
    function isAdminStatus() {
      if (user.isAdmin) return "✅";
      return "";
    }

    let deleteButton = button.cloneNode(false);
    deleteButton.innerText = "❌";
    deleteButton.setAttribute("onclick", `deleteUser(${user.id})`);

    let tr = tBody.insertRow();

    let td1 = tr.insertCell(0);
    td1.appendChild(isAdminCheckbox);

    let td2 = tr.insertCell(1);
    let textName = document.createTextNode(user.name);
    td2.appendChild(textName);

    let td3 = tr.insertCell(2);
    let textPassord = document.createTextNode(encode(user.password));
    td3.appendChild(textPassord);

    let td4 = tr.insertCell(3);
    td4.appendChild(deleteButton);
  });
  users = data;
}

function addUser() {
  let newUserName = document.getElementById("add-userName");
  let newUserPassword = document.getElementById("add-userPassword");
  var myHeaders = new Headers();
  myHeaders.append("Authorization", `Bearer ${Token}`);
  myHeaders.append("Content-Type", "application/json");
  var user = JSON.stringify({
    name: newUserName.value.trim(),
    isAdmin: false,
    password: newUserPassword.value.trim(),
  });

  var requestOptions = {
    method: "POST",
    headers: myHeaders,
    body: user,
    redirect: "follow",
  };

  fetch(`${url}`, requestOptions)
    .then((response) => response.text())
    .then(() => {
      newUserName.value = "";
      newUserPassword.value = "";
      getUsers();
    })
    .catch((error) => console.log("error", error));
}

function deleteUser(id) {
  var myHeaders = new Headers();
  myHeaders.append("Authorization", `Bearer ${Token}`);
  var requestOptions = {
    method: "DELETE",
    headers: myHeaders,
    redirect: "follow",
  };

  fetch(`${url}/${id}`, requestOptions)
    .then((response) => getUsers())
    .catch((error) => console.log("error", error));
}

// --------------- Tasks ---------------
const urlItems = "/MyTask";
getItems();
function getItems() {
  fetch(urlItems, {
    method: "Get",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
      Authorization: `Bearer ${Token}`,
    },
  })
    .then((response) => response.json())
    .then((data) => _displayItems(data))
    .catch((error) => alert("Unable to get items.", error));
}
function _displayItems(data) {
  const tBody = document.getElementById("tasks");
  tBody.innerHTML = "";

  _displayCountTask(data.length);

  var button = document.createElement("button");
  button.classList.add("TransparentButton");

  data.forEach((item) => {
    let isDoneCheckbox = document.createTextNode(isDoneStatus());
    function isDoneStatus() {
      if (item.isDone) return "👏";
      return "";
    }

    let editButton = button.cloneNode(false);
    editButton.innerText = "📝";
    editButton.setAttribute("onclick", `displayEditForm(${item.id})`);

    let deleteButton = button.cloneNode(false);
    deleteButton.innerText = "❌";
    deleteButton.setAttribute("onclick", `deleteItem(${item.id})`);

    let tr = tBody.insertRow();
    let td1 = tr.insertCell(0);
    td1.appendChild(isDoneCheckbox);

    let td2 = tr.insertCell(1);
    let textNode = document.createTextNode(item.name);
    td2.appendChild(textNode);

    let td3 = tr.insertCell(2);
    td3.appendChild(editButton);

    let td4 = tr.insertCell(3);
    td4.appendChild(deleteButton);
  });
  tasks = data;
}

function addItem() {
  const addNameTextbox = document.getElementById("add-task");

  const item = {
    isDone: false,
    name: addNameTextbox.value.trim(),
  };

  fetch(`${urlItems}`, {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
      Authorization: `Bearer ${Token}`,
    },
    body: JSON.stringify(item),
  })
    .then((response) => response.json())
    .then(() => {
      getItems();
      addNameTextbox.value = "";
    })
    .catch((error) => console.error("Unable to add item.", error));
}

function deleteItem(id) {
  fetch(`${urlItems}/${id}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${Token}`,
    },
  })
    .then(() => getItems())
    .catch((error) => console.error("Unable to delete item.", error));
}

function displayEditForm(id) {
  const item = tasks.find((item) => item.id === id);

  document.getElementById("edit-name").value = item.name;
  document.getElementById("edit-id").value = item.id;
  document.getElementById("edit-isDone").checked = item.isDone;
  document.getElementById("editForm").style.display = "block";
}

function updateItem() {
  const itemId = document.getElementById("edit-id").value;
  const item = {
    id: parseInt(itemId, 10),
    isdone: document.getElementById("edit-isDone").checked,
    name: document.getElementById("edit-name").value.trim(),
  };

  fetch(`${urlItems}/${itemId}`, {
    method: "PUT",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
      Authorization: `Bearer ${Token}`,
    },
    body: JSON.stringify(item),
  })
    .then(() => getItems())
    .catch((error) => console.error("Unable to update item.", error));

  closeInput();
  return false;
}

function closeInput() {
  document.getElementById("editForm").style.display = "none";
}

// --------------- Design ---------------
let open = false;
function showaddUserBlock() {
  const button = document.getElementById("addNewUserBlock");
  button.style.display = "block";
}

function showUserBlock() {
  if (open == true) {
    document.getElementById("blockUsersTable").style.display = "none";
    open = false;
  } else {
    document.getElementById("blockUsersTable").style.display = "block";
    open = true;
  }
  getUsers();
}

function encode(password) {
  let encoding = "";
  for (let index = 0; index < password.length; index++) encoding += "* ";
  return encoding;
}

function _displayCountTask(itemCount) {
  const name = itemCount <= 1 ? "task" : "tasks";
  document.getElementById("counter-task").innerText = `you have now ${itemCount} ${name}`;
}

function _displayCountUser(userCount) {
  const name = userCount <= 1 ? "user" : "users";
  document.getElementById("counter-user").innerText = `you have now ${userCount} ${name}`;
}
