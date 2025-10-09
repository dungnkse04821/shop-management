document.getElementById("addVariant").addEventListener("click", function () {
    var table = document.getElementById("variantsTable").getElementsByTagName('tbody')[0];
    var rowCount = table.rows.length;
    var row = table.insertRow(rowCount);

    row.innerHTML = `
                <td><input name="ViewModel.Product.Variants[${rowCount}].VariantName" class="form-control" /></td>
                <td><input name="ViewModel.Product.Variants[${rowCount}].Sku" class="form-control" /></td>
                <td><input name="ViewModel.Product.Variants[${rowCount}].Stock" type="number" class="form-control" /></td>
                <td><button type="button" class="btn btn-danger remove-variant">X</button></td>
            `;
});

document.addEventListener("click", function (e) {
    if (e.target && e.target.classList.contains("remove-variant")) {
        e.target.closest("tr").remove();
    }
});


document.addEventListener("click", function (e) {
    if (e.target && e.target.classList.contains("remove-image")) {
        e.target.closest("tr").remove();
    }
});

document.addEventListener("DOMContentLoaded", () => {
    const imageInput = document.getElementById("imageInput");
    const previewContainer = document.getElementById("previewContainer");

    if (!imageInput || !previewContainer) {
        console.warn("Không tìm thấy imageInput hoặc previewContainer trong DOM!");
        return;
    }

    let imagesData = []; // lưu danh sách ảnh tạm thời để dễ cập nhật sort order

    // ====== Khi chọn ảnh mới ======
    imageInput.addEventListener("change", () => {
        const files = Array.from(imageInput.files);

        files.forEach((file) => {
            if (!file.type.startsWith("image/")) return;

            const reader = new FileReader();
            reader.onload = (e) => {
                imagesData.push({
                    name: file.name,
                    url: e.target.result,
                    file: file
                });
                renderPreview();
            };
            reader.readAsDataURL(file);
        });

        // reset input để lần sau chọn lại được
        imageInput.value = "";
    });

    // ====== Hàm render lại toàn bộ preview ======
    function renderPreview() {
        previewContainer.innerHTML = "";

        imagesData.forEach((img, index) => {
            const wrapper = document.createElement("div");
            wrapper.classList.add("m-2", "text-center", "position-relative");
            wrapper.style.width = "130px";

            wrapper.innerHTML = `
                <img src="${img.url}"
                     alt="preview"
                     class="preview-img-thumbnail shadow-sm"
                     style="width: 120px; height: 120px; object-fit: cover; border-radius: 8px; border:0.5px solid #ccc; box-shadow: 0 2px 6px rgba(0,0,0,0.1);" />

                <button type="button"
                        class="btn btn-sm btn-danger position-absolute top-0 end-0 translate-middle"
                        style="border-radius: 50%; width: 25px; height: 25px; line-height: 1;"
                        data-index="${index}">
                    ×
                </button>

                <div class="small text-muted mt-1">#${index + 1}</div>

                <input type="hidden" name="Product.Images[${index}].SortOrder" value="${index + 1}" />
                <input type="hidden" name="Product.Images[${index}].FileName" value="${img.name}" />
            `;

            previewContainer.appendChild(wrapper);
        });

        // Gán lại sự kiện click cho nút xóa
        previewContainer.querySelectorAll(".btn-danger").forEach(btn => {
            btn.addEventListener("click", (e) => {
                const index = parseInt(e.currentTarget.getAttribute("data-index"));
                imagesData.splice(index, 1); // xóa ảnh khỏi mảng
                renderPreview(); // vẽ lại và cập nhật sort order
            });
        });
    }
});


document.addEventListener("DOMContentLoaded", function () {
    const thumbnails = document.querySelectorAll(".thumbnail-img");
    const carousel = document.querySelector("#productImageCarousel");

    // Đồng bộ thumbnail khi click
    thumbnails.forEach(tn => {
        tn.addEventListener("click", function () {
            thumbnails.forEach(t => t.classList.remove("active"));
            this.classList.add("active");
        });
    });

    // Khi carousel thay đổi slide, đổi thumbnail active
    carousel.addEventListener('slide.bs.carousel', function (e) {
        thumbnails.forEach(t => t.classList.remove("active"));
        thumbnails[e.to].classList.add("active");
    });

    // Đặt ảnh đầu tiên là active
    if (thumbnails.length > 0) {
        thumbnails[0].classList.add("active");
    }
});