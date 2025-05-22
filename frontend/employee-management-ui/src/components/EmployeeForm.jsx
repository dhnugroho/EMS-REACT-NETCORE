import { useState } from "react";
import API from "../services/api"; // Axios instance

export default function EmployeeForm() {
  const [form, setForm] = useState({
    firstName: "",
    middleName: "",
    lastName: "",
    dateOfBirth: "",
    gender: "",
    address: "",
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await API.post("/employees", form);
      alert("✅ Employee added successfully!");

      setForm({
        firstName: "",
        middleName: "",
        lastName: "",
        dateOfBirth: "",
        gender: "",
        address: "",
      });
    } catch (err) {
      console.error("Error adding employee:", err);
      alert("❌ Failed to add employee: " + (err.response?.data?.message || err.message));
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <input
        name="firstName"
        value={form.firstName}
        onChange={handleChange}
        className="w-full p-2 border rounded"
        placeholder="First Name"
        required
      />
      <input
        name="middleName"
        value={form.middleName}
        onChange={handleChange}
        className="w-full p-2 border rounded"
        placeholder="Middle Name (optional)"
      />
      <input
        name="lastName"
        value={form.lastName}
        onChange={handleChange}
        className="w-full p-2 border rounded"
        placeholder="Last Name"
        required
      />
      <input
        type="date"
        name="dateOfBirth"
        value={form.dateOfBirth}
        onChange={handleChange}
        className="w-full p-2 border rounded"
        required
      />
      <select
        name="gender"
        value={form.gender}
        onChange={handleChange}
        className="w-full p-2 border rounded"
        required
      >
        <option value="">Select Gender</option>
        <option>Male</option>
        <option>Female</option>
        <option>Other</option>
      </select>
      <input
        name="address"
        value={form.address}
        onChange={handleChange}
        className="w-full p-2 border rounded"
        placeholder="Address"
        required
      />
      <button
        type="submit"
        className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        Add Employee
      </button>
    </form>
  );
}