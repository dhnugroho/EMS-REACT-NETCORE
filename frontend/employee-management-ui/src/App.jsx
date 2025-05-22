import EmployeeForm from "./components/EmployeeForm";
import EmployeeList from "./components/EmployeeList";

function App() {
  return (
    <div className="max-w-4xl mx-auto mt-10 p-4">
      <h1 className="text-3xl font-bold mb-6">Employee Management</h1>

      <section className="mb-10">
        <h2 className="text-xl font-semibold mb-3">Add New Employee</h2>
        <EmployeeForm />
      </section>

      <section>
        <h2 className="text-xl font-semibold mb-3">Employee List</h2>
        <EmployeeList />
      </section>
    </div>
  );
}

export default App;
