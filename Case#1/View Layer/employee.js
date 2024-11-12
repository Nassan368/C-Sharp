$(() => {
    let isUpdating = false; // Track if we're updating or adding

    // Function to fetch all employees and build the list
    const getAll = async (msg) => {
        try {
            $("#employeeList").empty();
            $("#employeeList").text("Finding Employee Information...");

            let response = await fetch(`api/employee`);
            if (response.ok) {
                let payload = await response.json();
                buildEmployeeList(payload);
                $("#status").text(msg ? `${msg} - Employees Loaded` : "Employees Loaded");

                // Fetch department data
                response = await fetch(`api/department`);
                if (response.ok) {
                    let deps = await response.json();
                    sessionStorage.setItem("alldepartments", JSON.stringify(deps));
                } else if (response.status !== 404) {
                    let problemJson = await response.json();
                    errorRtn(problemJson, response.status);
                } else {
                    $("#status").text("No such path on server");
                }
            } else if (response.status !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {
                $("#status").text("No such path on server");
            }
        } catch (error) {
            $("#status").text(error.message);
        }
    };

    // Build the employee list dynamically
    const buildEmployeeList = (data) => {
        $("#employeeList").empty();
        let div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status">Employee Info</div>
                     <div class="list-group-item row d-flex text-center" id="heading">
                         <div class="col-4 h4">Title</div>
                         <div class="col-4 h4">First</div>
                         <div class="col-4 h4">Last</div>
                     </div>`);
        div.appendTo($("#employeeList"));

        let addBtn = $(`<button class="list-group-item row d-flex bg-success text-white" id="0">...click to add employee</button>`);
        addBtn.appendTo($("#employeeList"));

        sessionStorage.setItem("allemployees", JSON.stringify(data));
        data.forEach(emp => {
            let btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">
                            <div class="col-4">${emp.title}</div>
                            <div class="col-4">${emp.firstname}</div>
                            <div class="col-4">${emp.lastname}</div>
                         </button>`);
            btn.appendTo($("#employeeList"));
        });
    };

    // Setup modal for adding a new employee
    const setupForAdd = () => {
        $("#actionbutton").val("add").text("Add");
        $("#modaltitle").html("<h4>Add Employee</h4>");
        clearModalFields();
        $("#modalstatus").text("Add new employee");
        $("#theModal").modal("toggle");
        $("#deletebutton").hide(); // Hide delete button for adding
    };

    // Setup modal for updating an existing employee
    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("update").text("Update");
        $("#modaltitle").html("<h4>Update Employee</h4>");
        clearModalFields();
        data.forEach(employee => {
            if (employee.id === parseInt(id)) {
                $("#TextBoxTitle").val(employee.title);
                $("#TextBoxFirstName").val(employee.firstname);
                $("#TextBoxSurname").val(employee.lastname);
                $("#TextBoxEmail").val(employee.email);
                $("#TextBoxPhone").val(employee.phoneno);
                sessionStorage.setItem("employee", JSON.stringify(employee));
                $("#modalstatus").text("Update employee information");
                $("#theModal").modal("toggle");
                $("#deletebutton").show(); // Show delete button for updating
                loadDepartmentDDL(employee.departmentId);
            }
        });
    };

    const clearModalFields = () => {
        loadDepartmentDDL(-1);
        $("#TextBoxTitle, #TextBoxFirstName, #TextBoxSurname, #TextBoxEmail, #TextBoxPhone").val("");
        sessionStorage.removeItem("employee");
    };

    // Event handler for the "Add Employee" button
    $("#employeeList").on("click", "#0", () => {
        setupForAdd();
    });

    // Event handler for clicking an employee row for update
    $("#employeeList").on("click", (e) => {
        let id = $(e.target).closest("button").attr("id");
        let data = JSON.parse(sessionStorage.getItem("allemployees"));
        id === "0" ? setupForAdd() : setupForUpdate(id, data);
    });

    // Event handler for the action button (determines add or update)
    $("#actionbutton").on("click", () => {
        $("#actionbutton").val() === "update" ? update() : add();
    });

    // Add new employee
    const add = async () => {
        try {
            let emp = {
                title: $("#TextBoxTitle").val(),
                firstname: $("#TextBoxFirstName").val(),
                lastname: $("#TextBoxSurname").val(),
                email: $("#TextBoxEmail").val(),
                phoneno: $("#TextBoxPhone").val(),
                departmentId: parseInt($("#ddlDepartments").val()),
                id: -1,
                timer: null
            };

            let response = await fetch("api/employee", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(emp)
            });

            if (response.ok) {
                let data = await response.json();
                await getAll(`Employee ${$("#TextBoxSurname").val()} added`);
            } else if (response.status !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {
                $("#status").text("No such path on server");
            }
        } catch (error) {
            $("#status").text(error.message);
        }
        $("#theModal").modal("toggle"); // Close the modal after adding
    };

    // Update existing employee
    const update = async () => {
        try {
            let emp = JSON.parse(sessionStorage.getItem("employee"));
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirstName").val();
            emp.lastname = $("#TextBoxSurname").val();
            emp.email = $("#TextBoxEmail").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.departmentId = parseInt($("#ddlDepartments").val());

            let response = await fetch("api/employee", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(emp)
            });

            if (response.ok) {
                let data = await response.json();
                await getAll(data.msg); // Use the message from the server
            } else if (response.status !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {
                $("#status").text("No such path on server");
            }
            $("#theModal").modal("toggle");
        } catch (error) {
            $("#status").text(error.message);
        }
    };

    // Event handler to show delete confirmation dialog
    $("#deletebutton").on("click", () => {
        $("#dialog").show(); // Show the confirmation dialog
        $("#status").text(""); // Clear any previous status messages
    });

    // Event handler for delete confirmation - "Yes" button
    $("#yesbutton").on("click", async () => {
        $("#dialog").hide(); // Hide the dialog after clicking "Yes"
        await _delete(); // Call the delete function
    });

    // Event handler for delete confirmation - "No" button
    $("#nobutton").on("click", () => {
        $("#dialog").hide(); // Hide the confirmation dialog
        $("#status").text("Delete cancelled."); // Optional status message
    });

    // Delete function
    const _delete = async () => {
        const employee = JSON.parse(sessionStorage.getItem("employee"));
        if (employee && employee.id) {
            try {
                let response = await fetch(`api/employee/${employee.id}`, {
                    method: 'DELETE',
                    headers: { 'Content-Type': 'application/json; charset=utf-8' }
                });

                if (response.ok) {
                    await getAll(`Employee ${employee.id} deleted`);
                    $("#status").text(`Employee ${employee.id} deleted! - Employees Loaded`);
                } else {
                    let error = await response.json();
                    $("#status").text(`Error ${response.status}: ${error.message}`);
                }
            } catch (error) {
                $("#status").text(`Error: ${error.message}`);
            }
        } else {
            $("#status").text("No employee selected for deletion.");
        }
        $("#theModal").modal("toggle"); // Close the modal after deleting
    };


    // Load department dropdown list
    const loadDepartmentDDL = (deptId) => {
        let html = '';
        $('#ddlDepartments').empty();
        let alldepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
        alldepartments.forEach(dept => {
            html += `<option value="${dept.id}">${dept.departmentName}</option>`;
        });
        $('#ddlDepartments').append(html);
        $('#ddlDepartments').val(deptId);
    };

    // Error handling function
    const errorRtn = (problemJson, status) => {
        $("#status").text(`${problemJson.message} - ${status}`);
    };

    // Load all employees when the page loads
    getAll("");
});
