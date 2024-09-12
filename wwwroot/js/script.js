const apiUrl = window.location.origin;
let isEditMode = false;
let editMeetingId = null;

function loadMeetings() {
    fetch(`${apiUrl}/api/meetings/list`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            const meetingsTableBody = document.getElementById('meetingsTableBody');
            meetingsTableBody.innerHTML = '';

            data.forEach(meeting => {
                const row = document.createElement('tr');

                const meetingDate = new Date(meeting.date).toLocaleDateString();
                const startTime = meeting.startTime ? meeting.startTime : 'Belirtilmemiş';
                const endTime = meeting.endTime ? meeting.endTime : 'Belirtilmemiş';

                row.innerHTML = `
                                <td>${meeting.title}</td>
                                <td>${meetingDate}</td>
                                <td>${startTime}</td>
                                <td>${endTime}</td>
                                <td>${(meeting.participants && meeting.participants.length > 0) ? meeting.participants.join(', ') : 'Katılımcı yok'}</td>
                                <td>
                                    <button class="btn btn-warning btn-sm" onclick="editMeeting(${meeting.meetingId})">Düzenle</button>
                                    <button class="btn btn-danger btn-sm" onclick="deleteMeeting(${meeting.meetingId})">Sil</button>
                                </td>
                            `;
                meetingsTableBody.appendChild(row);
            });
        })
        .catch(error => {
            console.error('Hata:', error);
        });
}
document.getElementById('cancelButton').addEventListener('click', function () {
    resetForm();
    document.getElementById('meetingFormContainer').style.display = 'none';
});

document.getElementById('meetingForm').addEventListener('submit', function (event) {
    event.preventDefault();

    const title = document.getElementById('title').value;
    const date = document.getElementById('date').value;
    const today = new Date().toISOString().split('T')[0];

    const startTime = `${document.getElementById('start_time').value}:00`;
    const endTime = `${document.getElementById('end_time').value}:00`;


    if (date < today) {
        alert('Geçmiş bir tarih seçemezsiniz!');
        return;
    }
    const participants = Array.from(document.querySelectorAll('.participant'))
        .map(input => input.value.trim())
        .filter(p => p);

    const meetingData = {
        title: title,
        date: date,
        startTime: startTime,
        endTime: endTime,
        participants: participants
    };

    if (isEditMode && editMeetingId) {
        meetingData.meetingId = editMeetingId;

        fetch(`${apiUrl}/api/meetings/update/${editMeetingId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(meetingData)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {

                console.log('Toplantı başarıyla güncellendi:', data);
                isEditMode = false;
                editMeetingId = null;
                if (isEditMode) {
                    alert('Toplantı başarıyla güncellendi!');
                } else {
                    alert('Toplantı başarıyla kaydedildi!');
                }
                resetForm();
                document.getElementById('meetingFormContainer').style.display = 'none';
                loadMeetings();
            })
            .catch(error => {
                console.error('Hata:', error);
            });
    } else {
        fetch(`${apiUrl}/api/meetings/create`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(meetingData)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                return response.json();
            })
            .then(data => {
                alert('Toplantı başarıyla kaydedildi!');

                console.log('Toplantı başarıyla kaydedildi:', data);
                resetForm();
                document.getElementById('meetingFormContainer').style.display = 'none';
                loadMeetings();
            })
            .catch(error => {
                console.error('Hata:', error);
            });
    }
});

function editMeeting(id) {
    isEditMode = true;
    editMeetingId = id;
    document.getElementById('formTitle').innerText = "Toplantıyı Düzenle";
    document.getElementById('formSubmitButton').innerText = "Güncelle";

    fetch(`${apiUrl}/api/meetings/get/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(meeting => {
            document.getElementById('title').value = meeting.title;
            document.getElementById('date').value = meeting.date.split('T')[0];
            document.getElementById('start_time').value = meeting.startTime.slice(0, 5);
            document.getElementById('end_time').value = meeting.endTime.slice(0, 5);

            const participantsContainer = document.getElementById('participantsContainer');
            participantsContainer.innerHTML = '';
            if (meeting.participants && meeting.participants.length > 0) {
                meeting.participants.forEach(participant => {
                    const participantInputGroup = document.createElement('div');
                    participantInputGroup.className = 'input-group mb-2';
                    participantInputGroup.innerHTML = `
                                <input type="text" class="form-control participant" value="${participant}">
                                <button type="button" class="btn btn-danger remove-participant">Sil</button>
                            `;
                    participantsContainer.appendChild(participantInputGroup);

                    participantInputGroup.querySelector('.remove-participant').addEventListener('click', function () {
                        participantInputGroup.remove();
                    });
                });
            }
        })
        .catch(error => {
            console.error('Hata:', error);
        });

    document.getElementById('meetingFormContainer').style.display = 'block';
}
function resetForm() {
    document.getElementById('meetingForm').reset();
    const participantsContainer = document.getElementById('participantsContainer');
    participantsContainer.innerHTML = '';
}

document.getElementById('newMeetingButton').addEventListener('click', function () {
    resetForm();
    isEditMode = false;
    document.getElementById('formTitle').innerText = "Toplantı Oluştur";
    document.getElementById('formSubmitButton').innerText = "Toplantıyı Kaydet";
    document.getElementById('meetingFormContainer').style.display = 'block';
});

function deleteMeeting(id) {
    if (confirm('Bu toplantıyı silmek istediğinize emin misiniz?')) {
        fetch(`${apiUrl}/api/meetings/delete/${id}`, {
            method: 'DELETE'
        })
            .then(response => {
                loadMeetings();
            })
            .catch(error => {
                console.error('Hata:', error);
            });
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const today = new Date().toISOString().split('T')[0];
    document.getElementById('date').setAttribute('min', today);

    loadMeetings();
});

document.getElementById('addParticipant').addEventListener('click', function () {
    const participantsContainer = document.getElementById('participantsContainer');
    const participantInputGroup = document.createElement('div');
    participantInputGroup.className = 'input-group mb-2';
    participantInputGroup.innerHTML = `
                <input type="text" class="form-control participant" placeholder="Katılımcı Adı">
                <button type="button" class="btn btn-danger remove-participant">Sil</button>
            `;
    participantsContainer.appendChild(participantInputGroup);
    participantInputGroup.querySelector('.remove-participant').addEventListener('click', function () {
        participantInputGroup.remove();
    });
});
