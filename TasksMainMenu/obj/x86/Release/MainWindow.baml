   M S B A M L   `   `   `  ���� H  DTasksMainMenu, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null-clr-namespace:TasksMainMenuTasksMainMenu      TasksMainMenu.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *myclr-namespace:TasksMainMenu     -     � Title$   Functional Tasks Main Menu��$	��446��$	��903�� � WindowStartupLocation$ CenterScreen=� � 	Resources    taskDatasViewSource��%   &	        �� .���  � ColumnDefinitions �� $	��98* ��� $
��108* ��� $
��297* �.+��� -   $��	textBoxID��$
��Auto��$
��Left=�$��0,89,0,0q�$	��Top=�$	��108��$��1��$��20(��� -   $��ParticipantID��$��Participant ID��$
��Auto��$��Right=�$��0,89,0,0q�$	��Top=�$
��Auto��$��RightToLeft=�$��20(��� -   $��
textBoxAge��$��1��$
��Auto��$
��Left=�$��	0,140,0,0q�$	��Top=�$	��108��$��20(��� -   $	��Age��$	��Age��$
��Auto��$��Right=�$��	0,140,0,0q�$	��Top=�$��98��$��RightToLeft=�$��20(��� -   $��comboBoxGender��$��1��$
��Auto��$
��Left=�$��	0,185,0,0q�$	��Top=�$	��108�� �� Text$
 Male��$��20(�.@��� $
��Male���� $��Female���� -   $��Gender��$��Gender��$
��Auto��$��Right=�$��	0,185,0,0q�$	��Top=�$
��Auto��$��RightToLeft=�$��20(��� -   $��textBoxTimePoint��$��1��$
��Auto��$
��Left=�$��	0,246,0,0q�$	��Top=�$	��108��$��20(��� -	   $��	TimePoint��$��
Time Point��$
��Auto��$��Right=�$��	0,246,0,0q�$	��Top=�$
��Auto��$��RightToLeft=�$��20(��� -
   $��
buttonNext��$��Save and Go to Tasks��$��45��$
��Left=�$��
75,350,0,0q�$	��Top=�$	��198��$��2��$��20(��� -   $��exportDataBtn��$��Export All Data to CSV��$��45��$
��Left=�$��142,350,0,0q�$	��Top=�$	��210��$��2��$��20(��� -   $��exportDataInstructionLbl��$��2��$��28��$
��Left=�$��
92,128,0,0q�$	��Top=�$��[���� -   $��runTasksInstructionLbl��$��[]��$��2��$��28��$
��Left=�$��	36,31,0,0q�$	��Top=�$	��166���� -   $��expSummButton��$%��Export Task Summary Data to CSV��$��20(�$��45��$
��Left=�$��140,128,0,0q�$	��Top=�$	��307��$��2���� -   $��comboBoxSummary��$��2��$��33��$
��Left=�$��
138,75,0,0q�$	��Top=�$	��160��$��18(� � DataContext �� .@��� $	��ATM��$��26(��� $��Prescription Refill��$��26(��� $��Reaction Test��$��26(��� $��Forms��$��26(��� $��
DoctorTest��$��26(��� $��Metro Kiosk��$��26(��� $��OnlineBanking��$��26(�� -   $��grid1��$��2��$
��Left=�$��139,202,0,0q�$	��Top=� �� $
��Auto ��� $
��Auto � � RowDefinitions �� $
��Auto �.+��� $��Participant ID:��$��0��$��0��$
��Left=�$��3q�$��Center=�$��18(��� -   $��subjectIDTextBox��$��1��$��37��$
��Left=�$��3,3,0,0q�$�� ��$��Center=�$	��120��$��20(��� -   $��exportPartSumm��$'��!Export Participant Summary to CSV��$��2��$��40��$
��Left=�$��137,260,0,0q�$	��Top=�$	��319��$��20(��� -   $��
rectangle1��$��2��$	��364��$
��Left=�$��	10,31,0,0q�
���   �$	��Top=�$��3���� -   $��label1��$��Execute Tasks��$��39��$
��Left=�$��
118,12,0,0q�$	��Top=�$��2��$	��163��$��24(��� -   $��label2��$��Export Data��$��24(�$��39��$
��Left=�$��
178,12,0,0q�$	��Top=�$	��150��$��2���� -   $��
separator1��$��2��$��17��$
��Left=�$��
52,179,0,0q�$	��Top=�$	��460���� -   $��
separator2��$��2��$��1��$
��Left=�$��
52,327,0,0q�$	��Top=�$	��460���� -   $��image1��$��2��$��45��$
��Left=�$��	449,6,0,0q�$
��Fill=�$	��Top=�$��55��$��Resources/helpbutton.jpg��