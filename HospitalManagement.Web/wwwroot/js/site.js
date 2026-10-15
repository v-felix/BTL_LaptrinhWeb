document.addEventListener('submit', event => {
    const form = event.target;
    if (form.matches('form[data-confirm]') && !window.confirm(form.dataset.confirm)) event.preventDefault();
});
const currentPath = location.pathname.toLowerCase();
document.querySelectorAll('.sidebar a').forEach(a => { if (new URL(a.href).pathname.toLowerCase() === currentPath) a.classList.add('active'); });
const doctor = document.querySelector('[data-slot-doctor]');
const date = document.querySelector('[data-slot-date]');
const slots = document.querySelector('[data-slot-select]');
if (doctor && date && slots) {
    let request;
    async function reload() {
        request?.abort(); request = new AbortController();
        slots.replaceChildren(new Option('Đang tải giờ khám…', ''));
        slots.disabled = true;
        const feedback = document.getElementById('slot-feedback');
        try {
            if (!doctor.value || !date.value) { slots.replaceChildren(new Option('Chọn bác sĩ và ngày khám', '')); return; }
            const response = await fetch('/Appointment/Slots?' + new URLSearchParams({doctorId:doctor.value,date:date.value}), { signal:request.signal });
            if (!response.ok) throw new Error('Không thể tải lịch khám.');
            const data = await response.json();
            slots.replaceChildren(new Option(data.length ? 'Chọn giờ khám' : 'Không còn giờ khám trong ngày này', ''));
            data.forEach(s => slots.add(new Option(s.time + ' · ' + Number(s.fee).toLocaleString('vi-VN') + ' ₫', s.time)));
            if (feedback) feedback.textContent = data.length ? 'Phí khám được lấy từ lịch làm việc của bác sĩ.' : 'Vui lòng chọn ngày khác; ngày không có ca làm việc sẽ không có khung giờ.';
        } catch (error) {
            if (error.name === 'AbortError') return;
            slots.replaceChildren(new Option('Không tải được lịch. Hãy thử lại.', ''));
            if (feedback) feedback.textContent = 'Không thể kết nối máy chủ.';
        } finally { if (!request.signal.aborted) slots.disabled = false; }
    }
    doctor.addEventListener('change', reload); date.addEventListener('change', reload);
}
