// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const showModalMessage = (title, body) => {
    $("#lblModalTitle").text(title)
    $("#divModalBody").html(body)
    $("#modalMessage").modal()
}

const closeCurrentSession = () => {
    if (confirm("Desea cerrar la sesion actual?")) {
        location.href = "/Account/Logout";        
    }
}

const showAlertMessage = (type, body, url) => {
    $("#alertMessage").hide()
    $("#alertMessage").removeClass("alert-danger alert-info")

    $("#alertMessage").toggleClass(type === "Danger" ? "alert-danger" : "alert-info")
    $("#alertMessage").html(`${body}<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>`)
    $("#alertMessage").show()

    if (type === "Info" && url !== "") {
        setTimeout(() => {
            document.location.href = url
        }, 3000)
    }
}

const stringToHtml = (str) => {
    let parser = new DOMParser()
    let doc = parser.parseFromString(str, "text/html")
    return doc.body
}