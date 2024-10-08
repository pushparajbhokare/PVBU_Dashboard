async function signIn(){
    window.location.href="/dashboard";
}

async function clickrouteToCharacteristics(){
    window.location.href="/characteristics";
}

async function raiseModal(){
    const elem = document.getElementById("exampleModalCenter");
    elem.style.display="block";
    console.log("it ran");
    /*
    const elem=document.getElementById('qdasChart1');
    const navContainer=document.getElementById("sidenav-main");
    const modalContainer=document.getElementById("modalBackground");
    modalContainer.innerHTML = elem.innerHTML.slice();
    console.log(elem.innerHTML.slice());
    navContainer.style.display="none";
    modalContainer.style.display="flex";
    console.log('check what happened');
    */
};
function closeModal(){
    const elem = document.getElementById("exampleModalCenter");
    elem.style.display="none";
    console.log("it stop");
    /*
    const modalContainer=document.getElementById("modalBackground");
    modalContainer.style.display="none";
    const navContainer=document.getElementById("sidenav-main");
    navContainer.style.display="block";
    */
};