

const frontDiv = document.getElementsByClassName("front-pic")[0];
const backDiv = document.getElementsByClassName("back-pic")[0];
const frontInpFile = document.getElementById("fpic");
const backInpFile = document.getElementById("bpic");

const fImg = document.getElementById("img-front");
const bImg = document.getElementById("img-back");

frontDiv.addEventListener("click", function () {
    frontInpFile.click();
});

backDiv.addEventListener("click", function () {
    backInpFile.click();
});

frontInpFile.addEventListener("change", function () {

    const frontImage = this.files[0];

    if (frontImage) {
        const front_reader = new FileReader();

        front_reader.addEventListener("load", function () {
            console.log(this);

            fImg.style.display = "block";
            fImg.style.opacity = 1;

            fImg.setAttribute("src", this.result);
            document.getElementById("frontPicture").setAttribute("value", this.result);
        })

        
        front_reader.readAsDataURL(frontImage);
    }

});

backInpFile.addEventListener("change", function () {

    const backImage = this.files[0];

    if (backImage) {

        const back_reader = new FileReader();

        back_reader.addEventListener("load", function () {
            console.log(this);

            bImg.style.display = "block";
            bImg.style.opacity = "100%";

            bImg.setAttribute("src", this.result);
            document.getElementById("backPicture").setAttribute("value", this.result);
        })
        
        back_reader.readAsDataURL(backImage);
    }
});